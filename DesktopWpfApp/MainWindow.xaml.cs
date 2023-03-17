using DesktopWpfApp.SingOut;
using IdentityModel.OidcClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DesktopWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClientOptions options;
        private HttpClient httpclient;

        public MainWindow()
        {
            InitializeComponent();
            autologin.IsChecked = File.Exists(FilePath);
            if (autologin.IsChecked == true)
            {
                SignIn();
            }
        }

        private async Task SignIn()
        {
            Message1.Text = "Authenticating.....";
            // create a redirect URI using the custom redirect uri
            string redirectUri = string.Format("com.sciex" + "://call");
            Console.WriteLine("redirect URI: " + redirectUri);

            //var options = new OidcClientOptions
            //{
            //    Authority = "http://localhost:5003",
            //    ClientId = "mvc",
            //    Scope = "openid profile email offline_access user.create",
            //    ClientSecret = "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=",
            //    RedirectUri = "https://localhost:8000/Home/Signin-oidc",
            //    RefreshTokenInnerHttpHandler = new HttpClientHandler()
            //};

            //var options = new OidcClientOptions
            //{
            //    Authority = "https://dev-api.sciexcloud.net",
            //    ClientId = "UI_Client",
            //    Scope = "openid profile email offline_access user.create",
            //    ClientSecret = "rQttyyrUm2fs2ur8dgE5NTxQJbKEhPo3",
            //    RedirectUri = "http://localhost:4000/signin-oidc",
            //};

            this.options = new OidcClientOptions
            {
                Authority = "https://localhost:5001",
                ClientId = "mvc",
                Scope = "openid profile offline_access",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:8000/Home/Signin-oidc",
                RefreshTokenInnerHttpHandler = new HttpClientHandler()
            };

            var client = new OidcClient(options);

            var state = await client.PrepareLoginAsync();

            Console.WriteLine($"Start URL: {state.StartUrl}");

            var callbackManager = new CallbackManager(state.State);

            // open system browser to start authentication
            var psi = new ProcessStartInfo
            {
                FileName = state.StartUrl,
                UseShellExecute = true
            };
            Process.Start(psi);

            //Process.Start(state.StartUrl);

            Console.WriteLine("Running callback manager");
            var response = await callbackManager.RunServer();

            Console.WriteLine($"Response from authorize endpoint: {response}");

            // Brings the Console to Focus.
            //BringConsoleToFront();

            var result = await client.ProcessResponseAsync(response, state);

            //BringConsoleToFront();

            if (result.IsError)
            {
                Console.WriteLine("\n\nError:\n{0}", result.Error);
            }
            else
            {
                Console.WriteLine("\n\nClaims:");
                foreach (var claim in result.User.Claims)
                {
                    Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
                }

                Console.WriteLine();
                Console.WriteLine("Access token:\n{0}", result.AccessToken);

                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    Console.WriteLine("Refresh token:\n{0}", result.RefreshToken);
                }
            }

            var name = result.User.Identity.Name;
            Message1.Text = $"Hello {name}, you are logged in";

            this.httpclient = new HttpClient(result.RefreshTokenHandler);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:5001",
                ClientId = "mvc",
                Scope = "openid profile offline_access",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:8000/Home/Signin-oidc",
                PostLogoutRedirectUri = "https://localhost:8000/Home/Signin-oidc",
                Browser = new WpfEmbeddedBrowser()
            };

           var oidcClient = new OidcClient(options);
           await oidcClient.LogoutAsync();
           Message1.Text = $"you are logged out";
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //HttpContent stringContent = new StringContent(
            //    //       JsonConvert.SerializeObject(createRootUserDto), Encoding.UTF8, "application/json");
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri($"https://localhost:6001/identity"),
            //    //Content = stringContent
            //};

            try
            {
                var content = await httpclient.GetStringAsync("https://localhost:6001/identity");
                Message1.Text = JArray.Parse(content).ToString();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }            
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await SignIn();
        }

        private void autologin_Checked(object sender, RoutedEventArgs e)
        {
            if(autologin.IsChecked == true)
            {
                using var s = File.Create(FilePath);
            }
            else
            {
                try
                {
                    File.Delete(FilePath);
                }
                catch (Exception)
                {                   
                }
                
            }
        }

        public bool IsChecked
        {
            get
            {
                return File.Exists(FilePath);
            }
            set { }
        }

        private string FilePath => System.IO.Path.Combine(System.IO.Path.GetTempPath(), "autologin.txt");
    }
}
