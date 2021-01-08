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
    public class UserController : Controller
    {
        // GET: User
        string Root = ConfigurationManager.AppSettings["API_PATH"].ToString();
        string ProductKey = ConfigurationManager.AppSettings["PRODUCT_KEY"].ToString();
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
                HttpContext.Session["Username"] = usr.UserName;

                string ipAddr = GetIpAddress();
                string browserDet = GetBrowserDetails();
                string deviceDet = IsMobileDevice(Request.UserAgent) ? "Mobile" : "Computer";

                LicenseInputModel lstUser = new Models.LicenseInputModel()
                {
                    Login_ID = usr.UserName,
                    Password = usr.Password,
                    IPAddress = ipAddr,
                    DeviceType = deviceDet,
                    BrowserInfo = browserDet,
                    Product_Key = ProductKey
                };

                string basrURI = Root + "login";
                WebRequest request = WebRequest.Create(basrURI);
                if (request != null)
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.ContentType = "application/json";
                    request.Method = "POST";
                    var json = new JavaScriptSerializer().Serialize(lstUser);
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
                        return RedirectToAction("HomePage", "HomePage", lstUser);
                    }
                    else
                    {
                        ModelState.AddModelError("ErrMsg", respons.status_Description.ToString());
                    }
                }
            }
            return View(usr);
        }
        //[HttpPost]
        //public JsonResult AjaxMethod(string response)
        //{
        //    Recaptcha recaptcha = new Recaptcha();
        //    string key = ConfigurationManager.AppSettings["SecretKey"];
        //    string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + key + "&response=" + response;
        //    recaptcha.Response = (new WebClient()).DownloadString(url);
        //    return Json(recaptcha);
        //}
        //public string Home()
        //{
        //    return "Home Page";
        //}


        public ActionResult ViewUsers()
        {
            List<LICENSE_USER> lstUsers = new List<LICENSE_USER>();
            string Root = ConfigurationManager.AppSettings["API_PATH"].ToString();
            string ProductKey = ConfigurationManager.AppSettings["PRODUCT_KEY"].ToString();

            List<LICENSE_USER> lstLicenseInputModel = new List<LICENSE_USER>();
            string basrURI = Root + "GetAllUser?ProductKey=" + ProductKey;
            WebRequest request = WebRequest.Create(basrURI);
            if (request != null)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ContentType = "application/json";
                request.Method = "Get";
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string str = reader.ReadLine();
                if (str != "[]")
                {
                    JArray t = JArray.Parse(str);
                    lstLicenseInputModel = t.ToObject<List<LICENSE_USER>>();
                }

                if (lstLicenseInputModel != null && lstLicenseInputModel.Count > 0)
                {
                    return View(lstLicenseInputModel);
                }
            }
            return View(lstLicenseInputModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_Post(LICENSE_USER userDetails)
        {
            
                LICENSE_USER lstUser = new Models.LICENSE_USER()
                {
                    LOGIN_ID = userDetails.LOGIN_ID,
                    USER_NAME = userDetails.USER_NAME,
                    PASSWORD = userDetails.PASSWORD,
                    PRODUCT_KEY = ProductKey
                };

                string basrURI = Root + "AddUser";
                WebRequest request = WebRequest.Create(basrURI);
                if (request != null)
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.ContentType = "application/json";
                    request.Method = "POST";
                    var json = new JavaScriptSerializer().Serialize(lstUser);
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
                        return RedirectToAction("ViewUsers");
                    }
                    else
                    {
                        ViewBag.Message = respons.status_Description.ToString();
                    }
                }
            
            return View();
        }

        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            // Write delete script
            return RedirectToAction("ViewUsers");
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

    public class Recaptcha
    {
        public string Response { get; set; }
    }
    public class Response
    {
        public string status_code { get; set; } //0 failed 1 success
        public string status_Description { get; set; }
    }

}