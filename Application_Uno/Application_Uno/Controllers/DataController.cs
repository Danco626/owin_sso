using AuthWebApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Application_Uno.Controllers
{
    public class DataController : ApiController
    {
        private HttpClient client = new HttpClient();
        private async Task VerifyRefreshToken() 
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
           
            string expirationDateTime = claimsIdentity?.FindFirst(c => c.Type == "expires_in")?.Value;

            if (DateTime.Parse(expirationDateTime) < DateTime.Now)
            {
                string refreshToken = claimsIdentity?.FindFirst(c => c.Type == "refresh_token")?.Value;
                RefreshToken refreshResult = await RenewTokenAsync(refreshToken);
                DateTime newExpirationDateTime = DateTime.Now.AddSeconds(Convert.ToDouble(refreshResult.expires_in));                
                claimsIdentity.AddClaim(new Claim("expires_in", newExpirationDateTime.ToString()));
                claimsIdentity.AddClaim(new Claim("access_token2", refreshResult.access_token));
            }            
        }

        private async Task<RefreshToken> RenewTokenAsync(string refreshToken)
        {
            string domain = ConfigurationManager.AppSettings["domain"];
            string clientId = ConfigurationManager.AppSettings["client_id"];
            string clientSecret = ConfigurationManager.AppSettings["client_secret"];            

            var tokenPayload = new RefreshTokenRequest
            {  
                grant_type = "refresh_token",
                client_id = clientId,
                client_secret = clientSecret,
                refresh_token = refreshToken
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.PostAsJsonAsync($"https://{domain}/oauth/token", tokenPayload);
            return result.Content.ReadAsAsync<RefreshToken>().Result;
        }

        // GET: api/Data 
        public async Task<string> Get()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string ogRefreshToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;

            await VerifyRefreshToken();
            string newAccessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token2")?.Value;

            if (ogRefreshToken == newAccessToken)
            {
                return "access tokens match" ;
            }

            return newAccessToken;
        }     
    }
}
