using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
                string UserName = usr.UserName;
                
                HttpContext.Session["Username"] = usr.UserName;
                string ipAddr = GetIpAddress();
                string browserDet = GetBrowserDetails();
                string deviceType = IsMobileDevice(Request.UserAgent) ? "Mobile" : "Machine";
              return  RedirectToRoute(new { controller = "HomePage", action = "HomePage" });
                //return RedirectToRoute("HomePageController");
            }
            return View(usr);
        }
        [HttpPost]
        public JsonResult AjaxMethod(string response)
        {
            Recaptcha recaptcha = new Recaptcha();
            string key = ConfigurationManager.AppSettings["SecretKey"];
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + key + "&response=" + response;
            recaptcha.Response = (new WebClient()).DownloadString(url);
            return Json(recaptcha);
        }
        //public string Home()
        //{
        //    return "Home Page";
        //}

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

    public class Recaptcha
    {
        public string Response { get; set; }
    }

}