using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(User.Models.User usr)
        {
            if (ModelState.IsValid)
            {
                string ipAddr = GetIpAddress();
                string browserDet = GetBrowserDetails();
                string deviceType = IsMobileDevice(Request.UserAgent) ? "Mobile" : "Machine";

                return RedirectToAction("Home");
            }
            return View(usr);
        }
        public string Home()
        {
            return "Home Page";
        }

        private string GetIpAddress()
        {
            string ipAddress = this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = this.Request.ServerVariables["REMOTE_ADDR"];
            }
            //ipAddress = this.Request.UserHostAddress;
            return ipAddress;
        }

        private string GetBrowserDetails()
        {
            string userAgent = string.Empty;

            System.Web.HttpBrowserCapabilitiesBase hbc = HttpContext.Request.Browser;

            userAgent = hbc.Browser + " " + hbc.Version;

            return userAgent;
        }


        private bool IsMobileDevice(string userAgent)
        {
            string[] mobileDevices = new string[] {"iphone","ppc",
                                                      "windows ce","blackberry",
                                                      "opera mini","mobile","palm",
                                                      "portable","opera mobi" };
            userAgent = userAgent.ToLower();
            return mobileDevices.Any(x => userAgent.Contains(x));
        }
    }

}