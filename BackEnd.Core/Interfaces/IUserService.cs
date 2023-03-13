using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.DataLayer.Entities.User;

namespace BackEnd.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> Register(RegisterDTO model);

        Task<bool> EmailExist(string email);

        Task<bool> PhoneExist(string mobile);

        Task<User> LoginUser(LoginDTO model);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserById(long id);

        Task<UserInfoDTO> GetUserInfo(long id);

        Task<bool> CheckUserStatus(string code);

        Task<UserDashboardInfoDTO> GetUserDashboardInfo(long id);
        Task EditProfile(long id, EditProfileDTO model);
        Task<bool> ChangePassword(ChangePasswordDTO model);
    }

    public interface IAdminUserService
    {
        Task<UserAdminFilter> UserList(UserAdminFilter filter);

        Task<List<Role>> GetRoles();

        Task<long> AddUser(AddUserDTO model);

        Task AddRolesToUser(long id, List<int> rolesId);
        Task UpdateUserRoles(long id, List<int> rolesId);
        Task<List<GetUserRolesDTO>> getUserRoles();
        Task<List<int>> GetUserRolesByUserId(long id);
        Task<List<Role>> GetManyRolesById(List<int> roles);
        Task<GetUserDataForUpdate> GetUserForUpdate(long id);
        Task UpdateUser(UpdateUserDTO model);
        Task DeleteUser(long id);
    }
}
