using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Core.DTOs.User
{
    public class RegisterDTO
    {
        [Display(Name = "Username")]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Username { get; set; }

        [Display(Name = "Mobile number")]
        [Required]
        [StringLength(11, MinimumLength = 5)]
        public string Mobile { get; set; }

        [Display(Name = "Email")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(200, MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required]
        [StringLength(200, MinimumLength = 8)]
        [Compare("Password")]
        public string RePassword { get; set; }
    }

    public class LoginDTO
    {
        [Display(Name = "Email")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }

    public class UserInfoDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Image { get; set; }
    }

    public enum UserStatus
    {
        Enable,
        Disable,
        UserNotFound,
        AlreadyActivated
    }

    public class UserDashboardInfoDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public DateTime RegisterDate { get; set; }

        public int SuccessOrders { get; set; }

        public int Comments { get; set; }

        public int FailedOrders { get; set; }
    }

    public class EditProfileDTO
    {
        public string Username { get; set; }

        public string Mobile { get; set; }

        public string ImageAvatar { get; set; }

        public IFormFile? SelectedImage { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
