using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.IdentityServer.Config
{
    public class IdentityConfiguration
    {
        //public static IConfiguration Configuration { get; set; }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("clientservice", "CAS Client Service"),
                new ApiResource("productservice", "CAS Product Service"),
                new ApiResource("agentservice", "CAS Agent Service"),
                new ApiResource("Test_Api","TestService")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                //TODO:Set Client
                new Client
                {
                    ClientId="TestClient",
                    AllowAccessTokensViaBrowser=true,
                    ClientSecrets = new [] { new Secret("clientsecret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes=new []{ IdentityServerConstants.StandardScopes.OfflineAccess,"Test_Api" },
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=360000,
                    RefreshTokenExpiration=TokenExpiration.Sliding,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    UpdateAccessTokenClaimsOnRefresh=false,
                    AllowedCorsOrigins=new string[]
                    {
                        "http://localhost:54237",
                        "http://localhost:54238"
                    }
                },
                //new Client
                //{
                //    ClientId = "client.api.service",
                //    ClientSecrets = new [] { new Secret("clientsecret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = new [] { "clientservice" }
                //},
                //new Client
                //{
                //    ClientId = "product.api.service",
                //    ClientSecrets = new [] { new Secret("productsecret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = new [] { "clientservice", "productservice" }
                //},
                //new Client
                //{
                //    ClientId = "agent.api.service",
                //    ClientSecrets = new [] { new Secret("agentsecret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = new [] { "agentservice", "clientservice", "productservice" }
                //},
                //new Client
                //{
                //    ClientId = "cas.mvc.client.implicit",
                //    ClientName = "CAS MVC Web App Client",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    RedirectUris = { $"http://{Configuration["Clients:MvcClient:IP"]}:{Configuration["Clients:MvcClient:Port"]}/signin-oidc" },
                //    PostLogoutRedirectUris = { $"http://{Configuration["Clients:MvcClient:IP"]}:{Configuration["Clients:MvcClient:Port"]}/signout-callback-oidc" },
                //    AllowedScopes = new []
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "agentservice", "clientservice", "productservice"
                //    },
                //    AllowAccessTokensViaBrowser = true // can return access_token to this client
                //}
            };
        }
    }
}
