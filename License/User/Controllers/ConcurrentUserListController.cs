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
    public class ConcurrentUserListController : Controller
    {
        // GET: ConcurrentUserList
        
        public ActionResult Concurrent()
        {
            string Root = ConfigurationManager.AppSettings["API_PATH"].ToString();
            string ProductKey = ConfigurationManager.AppSettings["PRODUCT_KEY"].ToString();

            List<LicenseInputModel> lstLicenseInputModel = new List<LicenseInputModel>();
            string basrURI = Root + "GetAllLoggedUser?ProductKey=" + ProductKey;
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
                    lstLicenseInputModel = t.ToObject<List<LicenseInputModel>>();
                }
                
                if (lstLicenseInputModel !=null &&lstLicenseInputModel.Count > 0)
                {
                    return View(lstLicenseInputModel);
                }
               
            }
            return View(lstLicenseInputModel);
        }
    }
}