using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OrderApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("connectionstring")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}