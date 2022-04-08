using Intro_API_Web.Controllers.Data;
using Intro_API_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Intro_API_Web.Services
{
    public class AuthManager : IAuthManager
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var siginingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(siginingCredentials,claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials siginingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("").Value));
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuser").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: siginingCredentials
                );
            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {       
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["Jwt:Key"];
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginDTO loginDTO)
        {
            _user = await _userManager.FindByNameAsync(loginDTO.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, loginDTO.Password));
        }
    }
}
