using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

using Owin;

using System.Configuration;
using System.Security.Claims;
using Application_Uno.Support;

[assembly: OwinStartup(typeof(Application_Uno.Startup))]

namespace Application_Uno
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            string domain = ConfigurationManager.AppSettings["domain"];
            string clientId = ConfigurationManager.AppSettings["client_id"];
            string clientSecret = ConfigurationManager.AppSettings["client_secret"];
            string baseUrl = ConfigurationManager.AppSettings["baseURL"];
            string audience = ConfigurationManager.AppSettings["audience"];

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login"),
                CookieSameSite = SameSiteMode.Lax,
                CookieName = "Uno",
                // More information on why the CookieManager needs to be set can be found here: 
                // https://github.com/aspnet/AspNetKatana/wiki/System.Web-response-cookie-integration-issues
                CookieManager = new SameSiteCookieManager(new SystemWebCookieManager())
            });

            // Configure Auth0 authentication
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "Auth0",
                ResponseType = OpenIdConnectResponseType.Code,
                Authority = $"https://{domain}",

                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = $"{baseUrl}/callback",
                PostLogoutRedirectUri = baseUrl,
                Scope = "openid profile email offline_access",
                RedeemCode = true,
                
                
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                },

                // More information on why the CookieManager needs to be set can be found here: 
                // https://docs.microsoft.com/en-us/aspnet/samesite/owin-samesite
                CookieManager = new SameSiteCookieManager(new SystemWebCookieManager()),

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = notification =>
                    {
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("access_token", notification.ProtocolMessage.AccessToken));
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("refresh_token", notification.ProtocolMessage.RefreshToken));

                        DateTime expirationDateTime = DateTime.Now.AddSeconds(Convert.ToDouble(notification.ProtocolMessage.ExpiresIn));
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("expires_in", expirationDateTime.ToString()));
                      
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication) { 
                            notification.ProtocolMessage.SetParameter("audience", audience);
                        }

                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var logoutUri = $"https://{domain}/v2/logout?client_id={clientId}";

                            var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    // transform to absolute
                                    var request = notification.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                }
                                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                            }

                            notification.Response.Redirect(logoutUri);
                            notification.HandleResponse();
                        }
                        return Task.FromResult(0);
                    }
                }
            });         
        }        
    }
}
