using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using BackEnd.DataLayer.Context;
using BackEnd.DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Core.Services
{
    public class RoleService : IRoleService
    {
        #region constructor

        private SiteContext _context;

        public RoleService(SiteContext context)
        {
            _context = context;
        }

        #endregion
    }

    public class AdminRoleService : IAdminRoleService
    {
        #region constructor

        private SiteContext _context;

        public AdminRoleService(SiteContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.Where(r => !r.IsDelete).ToListAsync();
        }

        public async Task<List<Permission>> GetPermissions()
        {
            return await _context.Permission.ToListAsync();
        }

        public async Task AddPermissionToRole(int id, List<int> permissions)
        {
            foreach (var item in permissions)
            {
                var userPermission = await _context.Permission.FindAsync(item);
                if (userPermission != null)
                {
                    var rP = new RolePermission()
                    {
                        RoleId = id,
                        PermissionId = userPermission.Id
                    };
                    await _context.RolePermissions.AddAsync(rP);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<int> AddRole(AddRoleDTO model)
        {
            var role = new Role()
            {
                RoleName = model.Title
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }
    }
}
