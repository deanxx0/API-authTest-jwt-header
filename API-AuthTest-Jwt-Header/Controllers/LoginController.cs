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
            // user 별 member claim만 가진 토큰 생성, 커스텀헤더 access_token에 첨부
            // user 식별
            if (credential.UserName == "u" && credential.Password == "1")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                // secret key 설정, startup에 있는 key와 같아야함
                var key = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
                // 토큰에 넣을 정보 객체 설정
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // 토큰의 내용이 될 subject에 claim들을 추가
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        // 만들어질 토큰이 포함할 claim 정보들
                        new Claim(ClaimTypes.Name, "u"),
                        new Claim("Member", "true") 
                    }),
                    // 토큰 만료 설정
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                // 토큰정보객체를 이용하여 생성
                var token = tokenHandler.CreateToken(tokenDescriptor);
                // 토큰 스트링 획득
                var tokenString = tokenHandler.WriteToken(token);
                // 커스텀 응답헤더에 토큰 스트링 첨부
                HttpContext.Response.Headers.Add("access_token", tokenString);
                // Ok 응답과 함께 body에 토큰 스트링으로 응답
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
