using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License_API.Models
{
    public class LICENSE_LOG_HISTORY
    {
        [Key]
        public int ID { get; set; }
        public string LOGIN_ID { get; set; }
        public DateTime LOGINTIME { get; set; }
        public string IP_ADDRESS { get; set; }
        public string DEVICE_TYPE { get; set; }
        public string BROWSER_INFO { get; set; }
        public Nullable<DateTime> LOGOFFTIME { get; set; }
    }
}