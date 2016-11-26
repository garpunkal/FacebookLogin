using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using Newtonsoft.Json;

namespace FacebookLoginMVC.Controllers
{
    public class AccountController : Controller
    {   
        public ActionResult Login()
        {
            return View();
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }                
      
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "607914739302025",
                client_secret = "ce133c64e18d114d6e83536906c126af",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            try
            {
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "607914739302025",
                    client_secret = "ce133c64e18d114d6e83536906c126af",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                // Store the access token in the session
                Session["AccessToken"] = accessToken;

                // update the facebook client with the access token so 
                // we can make requests on behalf of the user
                fb.AccessToken = accessToken;

                // Get the user's information
                dynamic me = fb.Get("me?fields=first_name,last_name,id,email");
                string email = me.email;

                Session["me"] = me;

                // Set the auth cookie
                FormsAuthentication.SetAuthCookie(email, false);


                return RedirectToAction("Index", "Home");
            }
            catch { return RedirectToAction("Login", "Account"); }
        }
    }
}
