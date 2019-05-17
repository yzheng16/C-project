using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using WebApp.Admin.Security.DTOs;
using TeamCeBikeLab.Entities.POCOs;

namespace WebApp.Admin.Security.POCOs
{
    public class UnregisteredUser
    {
        public string Id { get; set; }
        public UserType UserType { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public string AssignedUserName { get; set; }
        public string AssignedEmail { get; set; }
    }
}