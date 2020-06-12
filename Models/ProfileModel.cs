using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSProfile.Models
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(12)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact is required")]
        public Int64 Contact { get; set; }
        [DisplayName("Birth date")]
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public int AccountID { get; set; }
        public string AccountType { get; set; }
        public List<ProfileModel> AccountList { get; set; }
    }
}