using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BackEnd.DataLayer.Entities.Product;

namespace BackEnd.DataLayer.Entities.User
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Username")]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Email { get; set; }

        [Display(Name = "Mobile number")]
        [Required]
        [StringLength(11, MinimumLength = 5)]
        public string Mobile { get; set; }

        [Display(Name = "Email activation code")]
        public string ActiveCode { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Avatar")]
        public string ImageAvatar { get; set; }

        #region Refrences

        public List<UserRole> UserRoles { get; set; }

        public List<Order> Orders { get; set; }

        public List<Comment> Comments { get; set; }

        #endregion
    }
}
