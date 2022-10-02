using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BackEnd.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;

namespace BackEnd.Core.DTOs.User
{
    public class UserAdminFilter : BasePaging
    {
        public string Email { get; set; }

        public List<DataLayer.Entities.User.User> UserList { get; set; }

        public UserAdminFilter SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.ActivePage = paging.ActivePage;
            return this;
        }
        public UserAdminFilter SetUser(List<DataLayer.Entities.User.User> user)
        {
            this.UserList = user;
            return this;
        }
    }

    public class AddUserDTO
    {
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

        public bool IsActive { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Password { get; set; }

        public string AvatarImage { get; set; }

        public List<int> Roles { get; set; }
    }

    public class GetUserRolesDTO
    {
        public string RoleName { get; set; }

        public long UserId { get; set; }
    }

    public class GetUserDataForUpdate
    {
        public long UserId { get; set; }

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

        public bool IsActive { get; set; }

        [Display(Name = "Password")]
        [StringLength(200, MinimumLength = 5)]
        public string Password { get; set; }

        public string AvatarImage { get; set; }

        public List<int> Roles { get; set; }
    }

    public class UpdateUserDTO
    {
        public long UserId { get; set; }

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

        public bool IsActive { get; set; }

        [Display(Name = "Password")]
        [StringLength(200, MinimumLength = 5)]
        public string Password { get; set; }

        public string AvatarImage { get; set; }

        public string SelectedImage { get; set; }

        public List<int> Roles { get; set; }
    }

    public class GetUserRolesForUpdate
    {
        public int RoleId { get; set; }
    }

    public class AddRoleDTO
    {
        public string Title { get; set; }

        public List<int> SelectedPermissions { get; set; }
    }
}
