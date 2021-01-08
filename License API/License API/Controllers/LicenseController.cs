using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using License_API.Models;
using License_API.Services;

namespace License_API.Controllers
{
    [RoutePrefix("api/License")]
    public class LicenseController : ApiController
    {
        LicenseServices l_services = new LicenseServices();
        [HttpPost]
        [Route("Login")]
        public object Login([FromBody] LicenseInputModel licenseInputModel)
        {
            var result = l_services.Login(licenseInputModel);
            return result;
        }

        [HttpPost]
        [Route("LogOff")]
        public object LogOff([FromBody] LicenseInputModel licenseInputModel)
        {
            var result = l_services.LogOff(licenseInputModel);
            return result;
        }

        [HttpPost]
        [Route("KeepAlive")]
        public object KeepAlive([FromBody] LicenseInputModel licenseInputModel)
        {
            var result = l_services.KeepAlive(licenseInputModel);
            return result;
        }

        [HttpPost]
        [Route("SessionExpire")]
        public object SessionExpire(string ProductKey)
        {
            var result = l_services.SessionExpire(ProductKey);
            return result;
        }

        [HttpGet]
        [Route("GetAllLoggedUser")]
        public object GetAllLggedUser(string ProductKey)
        {
            var result = l_services.GetAllLggedUser(ProductKey);
            return result;
        }

        [HttpPost]
        [Route("AddUser")]
        public object AddUser([FromBody] LICENSE_USER lICENSE_USER)
        {
            var result = l_services.AddUser(lICENSE_USER);
            return result;
        }

        [HttpGet]
        [Route("GetAllUser")]
        public object GetAllUser(string ProductKey)
        {
            var result = l_services.GetAllUser(ProductKey);
            return result;
        }

    }
}
