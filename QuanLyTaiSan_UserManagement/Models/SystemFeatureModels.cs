using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTaiSan_UserManagement.Models
{
    public class SystemFeature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }

    public class UserAuthorization
    {
        public string Id { get; set; }
        public int FeatureId { get; set; }
        public string RoleName { get; set; }
    }

    public class AspNetRoles
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    

    public class InformationUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }

}