using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace License_API.Models.License
{
    public class LICENSE_CONFIG
    {
        [Key]
        public string PRODUCT_KEY { get; set; }
        public int NUMBER_OF_USERS { get; set; }
        public int NUMBER_OF_LOGGING { get; set; }
        public int SESSION_TIME_INTRAVAL { get; set; }
        public string ISMULTI_lOGGIN { get; set; }
    }
}