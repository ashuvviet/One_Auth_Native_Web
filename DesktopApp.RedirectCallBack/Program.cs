// See https://aka.ms/new-console-template for more information
using DesktopWpfApp.RedirectCallBack;
using IdentityModel.Client;
using System.Diagnostics;

if (args.Any())
{
    await ProcessCallback(args[0]);
}


static async Task ProcessCallback(string args)
{
    //Debugger.Launch();
    var response = new AuthorizeResponse(args);
    if (!String.IsNullOrWhiteSpace(response.State))
    {
        Console.WriteLine($"Found state: {response.State}");
        var callbackManager = new CallbackManager(response.State);
        await callbackManager.RunClient(args);
    }
    else
    {
        Console.WriteLine("Error: no state on response");
    }
}