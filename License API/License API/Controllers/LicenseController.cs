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
    }
}
