using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.User
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Role name")]
        [Required]
        [MaxLength(200)]
        public string RoleName { get; set; }

        public bool IsDelete { get; set; }

        #region Refrences

        public List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

        #endregion
    }

    public class UserRole
    {
        [Key] 
        public long Id { get; set; }

        public int RoleId { get; set; }

        public long UserId { get; set; }

        public bool IsDelete { get; set; }

        #region Refrences

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}
