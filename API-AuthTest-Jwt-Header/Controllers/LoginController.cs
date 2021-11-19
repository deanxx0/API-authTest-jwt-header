using API_AuthTest_Jwt_Header.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_AuthTest_Jwt_Header.Controllers
{
    [Route("")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public LoginController()
        {

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Credential credential)
        {
            if (credential.UserName == "u" && credential.Password == "1")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "u"),
                        new Claim("Member", "true")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                HttpContext.Response.Headers.Add("access_token", tokenString);
                return Ok(tokenString);
            }
            if (credential.UserName == "admin" && credential.Password == "1")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim("Member", "true"),
                        new Claim("Admin", "true")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                HttpContext.Response.Headers.Add("access_token", tokenString);
                return Ok(tokenString);
            }


            return Unauthorized("try again");
        }
    }
}
