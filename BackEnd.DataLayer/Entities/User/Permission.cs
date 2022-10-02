using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.User
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public List<Permission> Permissions { get; set; }

        public List<RolePermission> RolePermissions { get; set; }
        #endregion
    }

    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}
