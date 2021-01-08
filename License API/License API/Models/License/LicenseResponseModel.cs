using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License_API.Models.License
{
    public class LicenseResponseModel
    {
        public string status_code { get; set; } //0 failed 1 success
        public string status_Description { get; set; }  
    }
}