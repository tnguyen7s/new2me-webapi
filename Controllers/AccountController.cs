using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using new2me_api.Data.Query;
using new2me_api.Dtos;
using new2me_api.Models;

namespace new2me_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IQuery query;
        private readonly IConfiguration configuration;
        public AccountController(IQuery query, IConfiguration configuration){
            this.configuration = configuration;
            this.query = query;
        }

        // POST api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> login(LoginReqDto loginReq){
            var user = await this.query.Authenticate(loginReq.Username, loginReq.Password);

            if (user==null){
                return Unauthorized();
            }

            var loginRes = new LoginResDto{
                Username = user.Username,
                Email = user.Email,
                PhoneNum = user.PhoneNum,
                Address = user.Address,
                NameOfUser = user.NameOfUser,
                Token = createJWT(user)
            };
            return Ok(loginRes);
        }

        private string createJWT(User user){
            // sign key
            var secretKey = configuration.GetSection("AppSettings:Key").Value;  
            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));

            // payload
            var claims = new Claim []{
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // sign algorithm
            var signingCredentials = new SigningCredentials(key,  SecurityAlgorithms.HmacSha256Signature); // can be changed into 512

            // subject + expire date + signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1), // changed later
                SigningCredentials = signingCredentials
            };

            // create token using token descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}