using HealthCare.Project.Core.Entities.Identity;
using HealthCare.Project.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Old
        //public Task<string> CreateTokenAsync(AppUser user)
        //{

        //    //var claims = new[]
        //    //{
        //    //    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    //    new Claim(ClaimTypes.Role, user.Role)
        //    //};
        //    var claims = new[]
        //     {
        //         new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //         new Claim(ClaimTypes.NameIdentifier, user.Id), // Add UserId
        //         new Claim(ClaimTypes.Role, user.Role)
        //     };
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(double.Parse( _configuration["Jwt:DurationInMinutes"])),
        //        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        //    );

        //    return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));


        //} 
        #endregion
        public Task<string> CreateTokenAsync(AppUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id), // User ID
                new Claim(ClaimTypes.Role, user.Role) // User Role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "DefaultSecretKey123456")); // Default key if missing

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "https://localhost:7120/",
                audience: _configuration["Jwt:Audience"] ?? "HealthCare",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"] ?? "60")),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
