using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using RedirectApp.Models;
using System.Diagnostics;
using System.IO.Pipes;

namespace RedirectApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ActionName("signin-oidc")]
        public async Task<IActionResult> signinoidc()
        {
            var b = HttpContext.Request.QueryString.Value;
            var response = new AuthorizeResponse(b);
            using (var client = new NamedPipeClientStream(".", response.State, PipeDirection.Out))
            {
                await client.ConnectAsync(15 * 1000);

                using (var sw = new StreamWriter(client) { AutoFlush = true })
                {
                    await sw.WriteAsync(b);
                }
            }

            return View();
            //return new EmptyResult();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}