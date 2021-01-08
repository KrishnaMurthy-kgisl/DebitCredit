using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace User.Models
{
    public class LicenseInputModel
    {
        [Display(Name = "Login ID")]
        public string Login_ID { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }
        [Display(Name = "Browser Info")]
        public string BrowserInfo { get; set; }
        [Display(Name = "System Name")]
        public string Product_Key { get; set; }
        [Display(Name = "Loggin Time")]
        public DateTime LogginTime { get; set; }
    }
}