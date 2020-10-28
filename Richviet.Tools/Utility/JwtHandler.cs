using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Richviet.Tools.Utility
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResource CreateAccessToken(int userId, string email,string name)
        {
            var claims = new Claim[]
            {
            new Claim("id", userId.ToString()),
            new Claim("email", email),
            new Claim("name", name)
            };
            var userClaimIdentity = new ClaimsIdentity(claims);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Subject = userClaimIdentity,
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Tokens:AccessExpireMinutes"])),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serilizedToken =  tokenHandler.WriteToken(securityToken);
            return CreateTokenResource(serilizedToken);
        }


        private static TokenResource CreateTokenResource(string token)
            => new TokenResource { Token = token };
    }
}
