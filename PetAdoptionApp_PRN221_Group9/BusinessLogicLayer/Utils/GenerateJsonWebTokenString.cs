using BusinessLogicLayer.Commons;
using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Utils
{
    public static class GenerateJsonWebTokenString
    {
        // target to User Object
        public static string GenerateJsonWebToken(this User user, AppConfiguration appsettingConfiguration, string SecretKey, DateTime time)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Create signature for header and payload
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.EmailAddress),
                /*new Claim(ClaimTypes.Role, user.Role.ToString()),*/
                new Claim("Role", user.Role.ToString()),
            }; 
            var token = new JwtSecurityToken(
                issuer: appsettingConfiguration.JWTSection.Issuer,
                audience: appsettingConfiguration.JWTSection.Audience,
                claims: claims,   //payload
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }




    }
}
