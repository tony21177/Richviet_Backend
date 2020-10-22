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

        public TokenResource CreateAccessToken(int userId, string email,string name,string countryForApp)
        {
            var now = DateTimeOffset.UtcNow;
            var iatString = now.ToUnixTimeSeconds().ToString();
            var expString = now.AddMinutes(int.Parse(_configuration["Tokens:AccessExpireMinutes"])).ToUnixTimeSeconds().ToString();
            Console.WriteLine(int.Parse(_configuration["Tokens:AccessExpireMinutes"]));
            Console.WriteLine(iatString);
            Console.WriteLine(expString);
            var claims = new Claim[]
            {
            new Claim("id", userId.ToString()),
            new Claim("email", email),
            new Claim("name", name),
            
            new Claim(JwtRegisteredClaimNames.Iat, iatString),
            new Claim("country", countryForApp)
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["tokens:accessexpireminutes"]));
            var jwt = CreateSecurityToken(claims, now.DateTime, expiry.DateTime, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token);
        }

        public TokenResource CreateRefreshToken(int userId)
        {
            var now = DateTimeOffset.UtcNow;
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString()),
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                SecurityAlgorithms.HmacSha256);
            var expiry = now.AddMinutes(double.Parse(_configuration["tokens:accessexpireminutes"]));
            var jwt = CreateSecurityToken(claims, now.DateTime, expiry.DateTime, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token);
        }

        private JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims, DateTime now, DateTime expiry, SigningCredentials credentials)
            => new JwtSecurityToken(claims: claims, notBefore: now,expires: expiry, signingCredentials: credentials);

        private static TokenResource CreateTokenResource(string token)
            => new TokenResource { Token = token };
    }
}
