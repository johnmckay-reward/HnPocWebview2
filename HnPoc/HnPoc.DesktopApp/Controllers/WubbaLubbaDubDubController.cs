using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace HnPoc.DesktopApp.Controllers
{
    [Route("api/wubba-lubba-dub-dub")]
    [ApiController]
    public class WubbaLubbaDubDubController : ControllerBase
    {
        public static MainWindow MainWindowInstance { get; set; }

        [HttpPost("update")]
        public bool Update(NewTextModel newTextModel)
        {
            var scriptToExecute = $"updateWubbaLubbaDubDub('{newTextModel.NewText}');";

            MainWindowInstance.Dispatcher.Invoke(() =>
            {
                MainWindowInstance.webView.ExecuteScriptAsync(scriptToExecute);
            });

            return true;
        }
    }

    public class NewTextModel
    {
        public string NewText { get; set; }
    }
}
