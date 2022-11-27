using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using new2me_api.Clients;
using new2me_api.Data.Query;
using new2me_api.Dtos;
using new2me_api.Helpers;
using new2me_api.Models;

namespace new2me_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IQuery query;
        private readonly IConfiguration configuration;
        private readonly IMailClient mailClient;
        private readonly IUserContext userContext;

        public AccountController(IQuery query, 
                                IConfiguration configuration, 
                                IUserContext userContext,
                                IMailClient mailClient){
            this.userContext = userContext;
            this.mailClient = mailClient;
            this.configuration = configuration;
            this.query = query;
        }

        // POST api/account/signup
        [HttpPost("signup")]
        public async Task<ActionResult> signup(SignupResDto signupReq){
            if (await this.query.UsernameExists(signupReq.Username)){
                return BadRequest("Username already exists, please try something else.");
            }

            if (await this.query.EmailExists(signupReq.Email)){
                return BadRequest("Email already exists, please try something else.");
            }

            var user = await this.query.SignUp(signupReq.Username, signupReq.Password, signupReq.Email);
           
            var loginRes = createLoginResponse(user);
            return Ok(loginRes);
        }

        // POST api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResDto>> login(LoginReqDto loginReq){
            var user = await this.query.Authenticate(loginReq.UsernameOrEmail, loginReq.Password);

            if (user==null){
                return Unauthorized();
            }

            var loginRes = createLoginResponse(user);
            return Ok(loginRes);
        }


        // POST api/account/resetPassRequest
        [HttpGet("resetPassRequest")]
        public async Task<IActionResult> requestResetPassword([FromQuery] string email){
            var user = await this.query.GetUserByEmail(email);
            if (user==null){
                return NotFound("Found no user.");
            }

            var token = this.createJWT(user);
            await this.mailClient.sendResetPassword(email, token);

            return Ok();
        }

        // POST api/account/resetPassword
        [HttpPost("resetPassword")]
        [Authorize]
        public async Task<IActionResult> resetPassword(ResetPasswordDto payload){
            // the new password
            var pwd = payload.Password;

            // get user based on their id
            var id = this.userContext.getUserID();
            var user = await this.query.GetUserById(id);
            if (user==null){
                return NotFound();
            }

            // check to make sure the new password is not the old one
            var hasUser = await this.query.Authenticate(user.Username, pwd);
            if (hasUser != null){
                return BadRequest("You are resetting with your current password. Please use another password to reset.");
            }

            // all succeed
            await this.query.resetUserPassword(user, pwd);
            
            return Ok(user);
        }


        // UPDATE api/account
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> updateAccount(UserDto userDto){
            var userId = this.userContext.getUserID();
            var user = await this.query.GetUserById(userId);

            if (user==null){
                return BadRequest("Update fails. No such user found.");
            }

            // check if there is another user that has the username=the one that the user wants to change
            var userOfUsername = await this.query.GetUserByUsername(userDto.Username);
            if (userOfUsername!=null && userOfUsername.Id!=userId){
                return BadRequest("Update fails. Username already exists.");
            }

            // check if there is another user that has the email=the one that the user wants to change
            var userOfEmail = await this.query.GetUserByEmail(userDto.Email);
            if (userOfEmail!=null && userOfEmail.Id!=userId){
                return BadRequest("Update fails. Email already exists");
            }

            user.Email = userDto.Email;
            user.Username = userDto.Username;
            user.Address = userDto.Address;
            user.PhoneNum = userDto.PhoneNum;
            user.NameOfUser = userDto.NameOfUser;

            await this.query.UpdateUser(user);
            
            var loginRes = createLoginResponse(user);
            return Ok(loginRes);
        }


        private LoginResDto createLoginResponse(User user){
            var loginRes = new LoginResDto{
                Username = user.Username,
                Email = user.Email,
                PhoneNum = user.PhoneNum,
                Address = user.Address,
                NameOfUser = user.NameOfUser,
                Expires =  new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUnixTimeMilliseconds(),
                Token = createJWT(user),
            };

            return loginRes;
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
                Expires = DateTime.UtcNow.AddDays(1), // changed later
                SigningCredentials = signingCredentials
            };

            // create token using token descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}