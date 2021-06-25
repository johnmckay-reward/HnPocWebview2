using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using HnPoc.DesktopApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace HnPoc.DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string UserDataFolderLocation = @"C:\Temp\HnPoc";
        private IHost _host;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CreateUserDataFolder();
                ConfigureAspNetCoreHost();

                this.webView.CreationProperties = new CoreWebView2CreationProperties()
                {
                    UserDataFolder = UserDataFolderLocation
                };
                this.webView.Source = new Uri("http://localhost:5000");

                this.webView.EnsureCoreWebView2Async();
                this.webView.CoreWebView2Ready += WebViewOnCoreWebView2Ready;

                WubbaLubbaDubDubController.MainWindowInstance = this;
            }
            catch (Exception e)
            {
                Application.Current.Shutdown();
            }
        }

        private static void CreateUserDataFolder()
        {
            if (Directory.Exists(UserDataFolderLocation)) return;

            Directory.CreateDirectory(UserDataFolderLocation);

            Thread.Sleep(500);
        }

        private void ConfigureAspNetCoreHost()
        {
            string appSettingsJsonFilename = "appsettings.json";

#if DEBUG
            appSettingsJsonFilename = "appsettings.Development.json";
#endif

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(x => x.AddJsonFile(appSettingsJsonFilename, false, true))
                .ConfigureWebHostDefaults(webHostBuilder => { webHostBuilder.UseUrls("http://0.0.0.0:5000").UseStartup<Startup>(); })
                .Build();

            _host.Start();
        }

        private void WebViewOnCoreWebView2Ready(object? sender, EventArgs e)
        {
            this.webView.CoreWebView2.Settings.IsWebMessageEnabled = true;
            this.webView.CoreWebView2.WebMessageReceived += CoreWebView2OnWebMessageReceived;
        }

        private void CoreWebView2OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var webMessage = e.TryGetWebMessageAsString();

            switch (webMessage)
            {
                case "Minimise":
                    WindowState = WindowState.Minimized;
                    break;
                case "Close":
                    Close();
                    break;
            }
        }

        private new static void Close()
        {
            var messageBoxResponse = MessageBox.Show("Are you sure you wish to close?", "Close HN Poc App", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResponse == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _host.Dispose();
            base.OnClosing(e);
        }
    }
}
