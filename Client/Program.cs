﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace ResourcesScopesConsoleClient
{
    class Program
    {
        private static DiscoveryCache Cache;

        static async Task Main(string[] args)
        {
            Console.Title = "Console Token Exchange Client";
            Cache = new DiscoveryCache("https://localhost:5001");

            // initial token
            var response = await RequestTokenAsync();
            var initialToken = response.AccessToken;

            "\n\nInitial token:".ConsoleYellow();
            response.Show();
            Console.ReadLine();

            response = await DelegateToken(initialToken, "impersonation");

            "\n\nImpersonation style:".ConsoleYellow();
            response.Show();
            HttpClient client = new HttpClient();
            client.SetBearerToken(response.AccessToken);
            var content = await client.GetStringAsync("https://localhost:6001/identity");
            Console.WriteLine(content);
            Console.ReadLine();

            //response = await DelegateToken(initialToken, "delegation");

            //"\n\nDelegation style:".ConsoleYellow();
            //response.Show();
            //Console.ReadLine();

            //response = await DelegateToken(initialToken, "custom");

            //"\n\nCustom style:".ConsoleYellow();
            //response.Show();
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await Cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "mvc",
                ClientSecret = "secret",

                Scope = "api1",
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task<TokenResponse> DelegateToken(string token, string style)
        {
            var client = new HttpClient();

            var disco = await Cache.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestTokenExchangeTokenAsync(new TokenExchangeTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "qc",
                ClientSecret = "secret",

                SubjectToken = token,
                SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken,
                Scope = "openid profile offline_access QCAPI",

                Parameters =
                {
                    { "exchange_style", style }
                }
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }
    }
}