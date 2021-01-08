using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using License_API.Models.License;

namespace License_API.Models
{
    public class DataBaseContext : DbContext
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public DataBaseContext() : base(connectionString)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
        }

        public virtual DbSet<LICENSE_USER> LICENSE_USER { get; set; }
        public virtual DbSet<LICENSE_LOG_HISTORY> LICENSE_LOG_HISTORY { get; set; }
        public virtual DbSet<LICENSE_CONFIG> LICENSE_CONFIG { get; set; }

    }
}