using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookLoginMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["me"] != null)
            {
                dynamic me = Session["me"];
                ViewBag.me = me;
                return View();
            }

            return RedirectToAction("Login", "Account");
        }
    }
}