# README #

Here is a very hastily hacked together POC demonstrating how to call WebView2 functions via an HTTP request.

To run:

* Install Dependencies/MicrosoftEdgeWebview2Setup.exe to get the runtime (this means that you don't actually need the new Edge installed on the machine, can be an old Windows 7 box with Internet Explorer, will still work)
* Open HnPoc.sln and build/run as normal
* Ignore the '1 or more projects couldn't be loaded correctly' error, no idea...
* Don't run as IIS express, run as HnPoc.DesktopApp
* When running, navigate to http://localhost:5000/swagger/index.html in your browser
* Calling /api/wubba-lubba-dub-dub/update will update the UI with the new text
* Clicking the close/minimise buttons will call native C# code

Any questions, email me at john@codebelfast.com :)
