using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace BackEnd.Web.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        #region constructor

        private IAdminRoleService _roleService;

        public RolesController(IAdminRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        [HttpGet("list")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(new { status = "Success", data = await _roleService.GetRoles()});
        }

        [HttpGet("get-permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            return Ok(new { status = "Success", data = await _roleService.GetPermissions() });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole(AddRoleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = "Error", message = "Validation error" });
            }

            var roleId = await _roleService.AddRole(model);
            await _roleService.AddPermissionToRole(roleId, model.SelectedPermissions);
            return Ok(new { status = "Success", message = "Role successfully added!" });
        }
    }
}
