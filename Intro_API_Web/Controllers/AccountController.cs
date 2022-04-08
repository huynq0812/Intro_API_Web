using AutoMapper;
using Intro_API_Web.Configurations;
using Intro_API_Web.Controllers.Data;
using Intro_API_Web.IRepository;
using Intro_API_Web.Models;
using Intro_API_Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;


        public AccountController(UserManager<ApiUser> userManager,
            //SignInManager<IdentityUser> signInManager,
            IMapper mapper, 
            ILogger<AccountController> logger, 
            IAuthManager authManager)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attemp for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest($"User register failed");
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something wrong in the {nameof(Register)}");
                return Problem($"Something wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation($"Login Attemp for {loginDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!await _authManager.ValidateUser(loginDTO))
                {
                    return Unauthorized();
                }
                return Accepted(new { Token = await _authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something wrong in the {nameof(Login)}");
                return Problem($"Something wrong in the {nameof(Login)}", statusCode: 500);
            }
        }

        //[HttpGet]
        //[Route("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    _logger.LogInformation($"Logout User");
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        await _signInManager.SignOutAsync();
        //        return Ok("Logged out");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something wrong in the {nameof(Logout)}");
        //        return Problem($"Something wrong in the {nameof(Logout)}", statusCode: 500);
        //    }
        //}

        [HttpPost]
        [Route("forget_password")]
        public async Task<IActionResult> ForgetPassWord(string email, string newPassword)
        {
            _logger.LogInformation($"Forget Password Attemp for {email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var currUser = await _userManager.FindByNameAsync(email);
                if (currUser == null)
                {
                    return Problem($"Don't have any email like this {email}");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(currUser);
                var result = await _userManager.ResetPasswordAsync(currUser, token, newPassword);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something wrong in the {nameof(ForgetPassWord)}");
                return Problem($"Something wrong in the {nameof(ForgetPassWord)}", statusCode: 500);
            }
        }     
    }
}
