using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyTaiSan_UserManagement
{

    // Serializable lưu thành secssion
    [Serializable]   
    public class UserLoginSec
    {
        public long UserID { set; get; }
        public string UserName { set; get; }
        public string GroupID { set; get; }
    }

    public class  InformationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleGroupID { get; set; }
        public string RoleGroupName { get; set; }
        public string FullName { get; set; }
       // public DateTime? LastLoginTime { get; set; }
    }

    public class RegisterUser
    {
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Quyền")]
        public string Role { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        public class CreateUserLogin
        {
            public long UserID { set; get; }
            public string UserName { set; get; }
            public string GroupID { set; get; }
        }
    }

}