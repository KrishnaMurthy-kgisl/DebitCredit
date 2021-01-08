using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult HomePage()
        {
            return View();
        }

        [HttpPost]
        [ActionName("HomePage")]
        public ActionResult Logout()
        {
            
                return RedirectToRoute(new { controller = "User", action = "Index" });
                //return RedirectToRoute("HomePageController");
            
        }
    }
}