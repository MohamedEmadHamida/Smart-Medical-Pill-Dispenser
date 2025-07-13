using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Entities.Identity;
using HealthCare.Project.Core.Services.Contract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;


        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }



        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return null;
            
            return new UserDto
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user),
                Role = user.Role
            };




        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExistAsync(registerDto.Email))
            {
                return new UserDto
                {
                    Email = "Email Is Already Registed"
                };
            };

            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Role = registerDto.Role,
               


            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return null;
            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user),
                DisplayName = user.Email.Split('@')[0],
                Role = user.Role  
            };
        }


        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
