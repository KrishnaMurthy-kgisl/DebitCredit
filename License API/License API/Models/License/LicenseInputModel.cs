using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License_API.Models
{
    public class LicenseInputModel
    {
        public string Login_ID { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string DeviceType { get; set; }
        public string BrowserInfo { get; set; }
        public string Product_Key { get; set; }
    }
}