using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApp.Admin.Security
{
    public static class Settings
    {
        //By turning these into properties with only a get;, no one will be able to change it on you
        public static string AdminRole => ConfigurationManager.AppSettings["adminRole"];
        public static string EmployeeRole => ConfigurationManager.AppSettings["employeeRole"];
        public static string CustomerRole => ConfigurationManager.AppSettings["customerRole"];
        public static string PurchasingRole => ConfigurationManager.AppSettings["purchasingRole"];
        public static string ServicesRole => ConfigurationManager.AppSettings["servicesRole"];
    }
}