using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using Backend.Core.Utilities.Extensions;
using BackEnd.Core.Utilities.Extensions;
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
                ActiveCode = Guid.NewGuid().ToString().Replace("-", "0"),
                IsActive = false,
                ImageAvatar = "user.png"
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

        public async Task<bool> CheckUserStatus(string code)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.ActiveCode == code);
            if (user != null)
            {
                user.IsActive = true;
                user.ActiveCode = Guid.NewGuid().ToString().Replace("-", "0");
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }

    public class AdminUserService : IAdminUserService
    {
        #region constructor

        private SiteContext _context;
        private IUserService _userService;

        public AdminUserService(SiteContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #endregion

        public async Task<UserAdminFilter> UserList(UserAdminFilter filter)
        {
            IQueryable<User> list = _context.Users.Where(u=>!u.IsDelete);
            if (!String.IsNullOrEmpty(filter.Email))
            {
                list = list.Where(u => u.Email.Contains(filter.Email.Trim().ToLower()));
            }
            filter.UserList = await list.ToListAsync();
            var count = (int)Math.Ceiling(list.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var user = await list.Paging(pager).ToListAsync();
            return filter.SetUser(user).SetPaging(pager);
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<long> AddUser(AddUserDTO model)
        {
            var user = new User()
            {
                Username = model.Username.Trim(),
                Mobile = model.Mobile.Trim(),
                Email = model.Email.Trim().ToLower(),
                Password = PasswordHelper.EncodePasswordMd5(model.Password.Trim()),
                ActiveCode = Guid.NewGuid().ToString().Replace("-", ""),
                IsActive = model.IsActive,
                ImageAvatar = "user.png"
            };

            if (!string.IsNullOrEmpty(model.AvatarImage))
            {
                var imageFile = ImageUploaderExtension.Base64ToImage(model.AvatarImage);
                var imageName = Guid.NewGuid().ToString("N") + ".jpg";
                imageFile.AddImageToServer(imageName, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/"));
                user.ImageAvatar = imageName;
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task AddRolesToUser(long id, List<int> rolesId)
        {
            foreach (var item in rolesId)
            {
                var userRole = await _context.Roles.FindAsync(item);
                var uR = new UserRole()
                {
                    RoleId = item,
                    UserId = id,
                    IsDelete = false,
                };
                await _context.UserRoles.AddAsync(uR);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserRoles(long id, List<int> rolesId)
        {
            await _context.UserRoles.Where(ur => ur.UserId == id).ForEachAsync(ur => _context.UserRoles.Remove(ur));
            await AddRolesToUser(id, rolesId);
        }

        public async Task<List<GetUserRolesDTO>> getUserRoles()
        {

            return await _context.UserRoles.Include(ur=>ur.Role).Select(ur => new GetUserRolesDTO()
            {
                RoleName = ur.Role.RoleName,
                UserId = ur.UserId
            }).ToListAsync();
        }

        public async Task<List<int>> GetUserRolesByUserId(long id)
        {
            return await _context.UserRoles.Include(ur=>ur.Role).Where(ur => ur.UserId == id).Select(ur=>ur.RoleId).ToListAsync();
        }

        public async Task<List<Role>> GetManyRolesById(List<int> roles)
        {
            return await _context.Roles.Where(r => roles.Contains(r.Id)).ToListAsync();
        }

        public async Task<GetUserDataForUpdate> GetUserForUpdate(long id)
        {
            var user = await _userService.GetUserById(id);
            var data = new GetUserDataForUpdate()
            {
                UserId = user.Id,
                Username = user.Username,
                Mobile = user.Mobile,
                Email = user.Email,
                AvatarImage = user.ImageAvatar != null ? user.ImageAvatar : "user.png",
                IsActive = user.IsActive,
                Roles = await GetUserRolesByUserId(id)
            };
            return data;
        }

        public async Task UpdateUser(UpdateUserDTO model)
        {
            var user = await _userService.GetUserById(model.UserId);

            user.Username = model.Username;
            user.Mobile = model.Mobile;
            user.Email = model.Email;
            user.IsActive = model.IsActive;
            user.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
            user.ImageAvatar = model.AvatarImage;
            if (model.Password != null)
            {
                user.Password = PasswordHelper.EncodePasswordMd5(model.Password);
            }
            if (!string.IsNullOrEmpty(model.SelectedImage))
            {
                var imageName = "";
                var imageFile = ImageUploaderExtension.Base64ToImage(model.SelectedImage);
                if (model.AvatarImage == "user.png")
                {
                    imageName = Guid.NewGuid().ToString("N") + ".jpg";
                }
                else
                {
                    imageName = model.AvatarImage;
                }
                imageFile.AddImageToServer(imageName, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/"));
                user.ImageAvatar = imageName;
            }

            await UpdateUserRoles(model.UserId, model.Roles);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(long id)
        {
            var user = await _userService.GetUserById(id);
            user.IsDelete = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
