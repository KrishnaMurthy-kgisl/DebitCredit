using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using User.Models;

namespace User.Controllers
{
    public class HomePageController : Controller
    {
        string Root = ConfigurationManager.AppSettings["API_PATH"].ToString();
        string ProductKey = ConfigurationManager.AppSettings["PRODUCT_KEY"].ToString();

        // GET: HomePage
        public ActionResult HomePage(LicenseInputModel l)
        {
            return View(l);
        }

        [HttpPost]
        [ActionName("HomePage")]
        public ActionResult Logout(LicenseInputModel l)
        {
            string basrURI = Root + "LogOff";
            WebRequest request = WebRequest.Create(basrURI);
            if (request != null)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = "application/json";
                request.Method = "POST";
                var json = new JavaScriptSerializer().Serialize(l);
                byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string str = reader.ReadLine();
                JObject t = JObject.Parse(str);
                Response respons = t.ToObject<Response>();
                if (respons.status_code == "1")
                {
                    return RedirectToAction("Index", "User");

                }
            }
            return RedirectToAction("HomePage", l);
        }
    }
}