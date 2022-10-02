using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.User;
using BackEnd.Core.Interfaces;
using BackEnd.Core.Services;
using BackEnd.Core.Utilities.Extensions;
using BackEnd.Core.Utilities.Security;
using BackEnd.DataLayer.Entities.User;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Text;

namespace BackEnd.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Constructor

        private IUserService _userService;
        private IViewRenderService _renderService;

        public AccountController(IUserService userService, IViewRenderService renderService)
        {
            _userService = userService;
            _renderService = renderService;
        }

        #endregion


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new {status = "Invalid", message = "Data is invalid"});
            }

            if (await _userService.EmailExist(model.Email))
            {
                return BadRequest(new { status = "Email exists" , message = "Your email already registered" });
            }
            if (await _userService.PhoneExist(model.Mobile))
            {
                return BadRequest(new { status = "Mobile exists", message = "Your mobile number already registered" });
            }

            var user = await _userService.Register(model);
            
            string body = _renderService.RenderToStringAsync("_EmailTemp", user);
            EmailSender.Send(model.Email.ToLower().Trim(), "Activate account", body);
            return Ok(new {status = "Success", message = "You are successfully registered!"});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var res = await _userService.LoginUser(model);
            if (res != null)
            {
                if (res.IsActive)
                {
                    var user = await _userService.GetUserByEmail(model.Email);
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularDigiShopJwtBearer344783"));
                    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:44308/",
                        claims: new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, model.Email)
                        },
                        expires: model.IsRemember ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(2),
                        signingCredentials: signInCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new { token = tokenString, expireTime = 30, id = user.Id, username = user.Username, email = user.Email, status = "Success" });
                }
                return Ok(new { status = "Deactivated", description = "The user is suspended" });
            }

            return Ok(new { status = "NotFound", description = "The user was not found" });
        }

        [HttpGet("activate-account/{code}")]
        public async Task<IActionResult> ActivateAccount(string code)
        {
            if (await _userService.CheckUserStatus(code))
            {
                return Ok(new {status = "Success", message = "account activated"});
            }
            return BadRequest(new {status = "NotFound", message = "user not found"});
        }

        [HttpPost("check-auth")]
        public async Task<IActionResult> CheckAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                var user = await _userService.GetUserById(userId);
                return Ok(new { status = "Success", data = new { id = user.Id, email = user.Email, username = user.Username } });
            }
            return Ok(new { status = "UnAuthorized" });
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            return Ok(new {status = "Success", data = await _userService.GetUserById(id)});
        }

        [HttpGet("get-sidebar-info/{id}")]
        public async Task<IActionResult> GetSidebarInfoById(long id)
        {
            return Ok(new { status = "Success", data = await _userService.GetUserInfo(id) });
        }

        [HttpGet("get-dashboard-info/{id}")]
        public async Task<IActionResult> GetDashboardInfo(long id)
        {
            return Ok(new { status = "Success", data = await _userService.GetUserDashboardInfo(id) });
        }
    }
}