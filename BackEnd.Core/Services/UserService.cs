using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Utilities.Security;
using BackEnd.DataLayer.Context;
using BackEnd.DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Core.Services
{
    public class UserService : IUserService
    {
        #region constructor

        private SiteContext _context;

        public UserService(SiteContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<User> Register(RegisterDTO model)
        {
            var user = new User()
            {
                Username = model.Username,
                Email = model.Email.Trim().ToLower(),
                Mobile = model.Mobile,
                Password = PasswordHelper.EncodePasswordMd5(model.Password),
                RegisterDate = DateTime.Now,
                ActiveCode = Guid.NewGuid().ToString().Replace("-","0"),
                IsActive = false
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email.ToString().ToLower().Trim());
        }

        public async Task<bool> PhoneExist(string mobile)
        {
            return await _context.Users.AnyAsync(u => u.Mobile == mobile);
        }

        public async Task<User> LoginUser(LoginDTO model)
        {
            return await _context.Users.SingleOrDefaultAsync(u =>
                u.Email == model.Email.Trim().ToLower() &&
                u.Password == PasswordHelper.EncodePasswordMd5(model.Password));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email.ToLower().Trim());
        }

        public async Task<User> GetUserById(long id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserInfoDTO> GetUserInfo(long id)
        {
            var user = await GetUserById(id);
            var info = new UserInfoDTO()
            {
                Email = user.Email,
                Username = user.Username,
                Mobile = user.Mobile
            };
            return info;
        }
    }
}
