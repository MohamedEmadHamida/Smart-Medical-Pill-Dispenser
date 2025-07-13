using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Entities.Identity;
using HealthCare.Project.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        

        public AccountsController(IUserService userService, UserManager<AppUser> userManager, IEmailService emailService, ITokenService tokenService)
        {
            _userService = userService;
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
        }
        

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized();
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user is null ) return BadRequest("Invalid SignUp Registeration");
            return Ok(user);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if ((user != null))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgetpasswordlink = Url.Action(nameof(ResetPassword), "Accounts", new { token, user.Email }, Request.Scheme);

                var emailDto = new EmailDto()
                {
                    To = email,
                    Subject = "Reset Password",
                    Body = forgetpasswordlink


                };

                _emailService.SendEmail(emailDto);

                return Ok($"Success Email Sent to {email} SuccessFully");

            }
            return BadRequest("Invalid Operation");
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDto() { Token = token, Email = email };

            return Ok(new { model });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmedPassword)
            {
                return BadRequest("New password and confirmation password do not match.");
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user != null)
            {
                // Attempt to reset the password
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

                if (!resetPassResult.Succeeded)
                {
                    // Add errors to the ModelState and return them
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return Ok("Password has been changed.");
            }

            return BadRequest("Cannot change password, please try again!");
        }


        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            return Ok("User signed out successfully");
        }
    }
}
