using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace User.Models
{
    public class LICENSE_USER
    {
        public int USERID { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string USER_NAME { get; set; }
        [Required]
        [Display(Name = "Login ID")]
        public string LOGIN_ID { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string PASSWORD { get; set; }
        public string ACTIVESTATUS { get; set; }
        public string PRODUCT_KEY { get; set; }
    }
}