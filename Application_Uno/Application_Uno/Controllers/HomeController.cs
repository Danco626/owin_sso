using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Application_Uno.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Login(string returnUrl = "/")
        //{
        //    HttpContext.GetOwinContext().Authentication.Challenge(
        //      new AuthenticationProperties
        //      {
        //          RedirectUri = returnUrl ?? Url.Action("Index", "Home")
        //      },
        //      "Auth0"
        //    );

        //    return new HttpUnauthorizedResult();
        //}

        ////public ActionResult Callback()
        ////{

        ////    return View("Home");
        ////}


        ////[Authorize]
        ////public ActionResult UserProfile()
        ////{
        ////    var claimsIdentity = User.Identity as ClaimsIdentity;

        ////    return View(new UserProfileViewModel()
        ////    {
        ////        Name = claimsIdentity?
        ////          .FindFirst(c => c.Type == claimsIdentity.NameClaimType)?.Value,
        ////        EmailAddress = claimsIdentity?
        ////          .FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
        ////        ProfileImage = claimsIdentity?
        ////          .FindFirst(c => c.Type == "picture")?.Value
        ////    });
        ////}

        //[Authorize]
        //public ActionResult Tokens()
        //{
        //    var claimsIdentity = User.Identity as ClaimsIdentity;

        //    // Extract tokens
        //    string accessToken = claimsIdentity?.FindFirst(c => c.Type == "access_token")?.Value;
        //    string idToken = claimsIdentity?.FindFirst(c => c.Type == "id_token")?.Value;

        //    // Now you can use the tokens as appropriate...
        //    return View();
        //}

        //[Authorize]
        //public void Logout()
        //{
        //    HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        //    HttpContext.GetOwinContext().Authentication.SignOut("Auth0");
        //}
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}