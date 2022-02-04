using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Utilities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Web.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region constructor

        private IAdminUserService _adminUserService;

        public UserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }


        #endregion


        [HttpGet("user-list")]
        public async Task<IActionResult> Index([FromQuery] UserAdminFilter filter)
        {
            return Ok(new {status = "Success", data = await _adminUserService.UserList(filter)});
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(new {status = "Success", data = await _adminUserService.GetRoles()});
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser(AddUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new {status = "Error", message = "Data validation error"});
            }

            long id = await _adminUserService.AddUser(model);
            await _adminUserService.AddRolesToUser(id, model.Roles);
            return Ok(new {status = "Success", message = "user successfully added!"});
        }

        [HttpGet("user-roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            return Ok(new {status = "Success", data = await _adminUserService.getUserRoles()});
        }

        [HttpGet("update-user-roles/{id}")]
        public async Task<IActionResult> GetUserRolesForUpdate([FromRoute] long id)
        {
            return Ok(new { status = "Success", data = await _adminUserService.GetUserRolesByUserId(id) });
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserData([FromRoute] long id)
        {
            return Ok(new {status = "Success", data = await _adminUserService.GetUserForUpdate(id)});
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new {status = "Error", message = "Validation error!"});
            }

            await _adminUserService.UpdateUser(model);
            return Ok(new {status = "Success", message = "User has been updated successfully!"});
        }
        
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _adminUserService.DeleteUser(id);
            return Ok(new {status = "Success", message = "User has been removed successfully!"});
        }
    }
}