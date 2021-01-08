using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License_API.Models
{
    public class LICENSE_USER
    {
        [Key]
        public int USERID { get; set; }
        public string USER_NAME { get; set; }
        public string LOGIN_ID { get; set; }
        public string PASSWORD { get; set; }
        public string ACTIVESTATUS { get; set; }
        public string Product_Key { get; set; }
    }
}