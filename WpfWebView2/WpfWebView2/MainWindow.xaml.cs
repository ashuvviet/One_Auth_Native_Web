using IdentityModel.OidcClient;
using System;
using System.Net.Http;
using System.Windows;

namespace WpfWebView2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClient _oidcClient;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string redirectUri = string.Format("sample-windows-client" + "://callback");

            var options = new OidcClientOptions
            {
                Authority = "https://localhost:5001",
                ClientId = "mvc",
                Scope = "openid profile offline_access",
                ClientSecret = "secret",
                RedirectUri = redirectUri,
                Browser = new WpfEmbeddedBrowser(),
                Policy = new Policy
                {
                    RequireIdentityTokenSignature = false
                },
                RefreshTokenInnerHttpHandler = new HttpClientHandler()
            };

            //var options = new OidcClientOptions
            //{
            //    Authority = "https://localhost:5001",
            //    ClientId = "mvc",
            //    Scope = "openid profile offline_access",
            //    ClientSecret = "secret",
            //    RedirectUri = redirectUri,
            //    Browser = new WpfEmbeddedBrowser(),
            //    Policy = new Policy
            //    {
            //        RequireIdentityTokenSignature = false
            //    }
            //};

            _oidcClient = new OidcClient(options);

            LoginResult loginResult;
            try
            {
                loginResult = await _oidcClient.LoginAsync();
            }
            catch (Exception exception)
            {
                txbMessage.Text = $"Unexpected Error: {exception.Message}";
                return;
            }

            if (loginResult.IsError)
            {
                txbMessage.Text = loginResult.Error == "UserCancel" ? "The sign-in window was closed before authorization was completed." : loginResult.Error;
            }
            else
            {
                txbMessage.Text = loginResult.User.Identity.Name;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = await _oidcClient.LogoutAsync();
            //var loginResult = await _oidcClient.LoginAsync();
        }
    }
}
