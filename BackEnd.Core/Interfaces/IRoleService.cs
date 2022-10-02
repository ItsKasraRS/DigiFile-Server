using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.DataLayer.Entities.User;

namespace BackEnd.Core.Interfaces
{
    public interface IRoleService
    {
        
    }

    public interface IAdminRoleService
    {
        Task<List<Role>> GetRoles();
        Task<List<Permission>> GetPermissions();
        Task AddPermissionToRole(int id, List<int> permissions);
        Task<int> AddRole(AddRoleDTO model);
    }
}
