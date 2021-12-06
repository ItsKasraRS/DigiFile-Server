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
    }
}
