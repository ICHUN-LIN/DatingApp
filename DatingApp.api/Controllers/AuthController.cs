using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.api.Data;
using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.api.DTOS;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //help input validation
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
            this._repo = repository;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO user)
        {
            //[FromBody] let mvc core know where user come from
            user.UserName = user.UserName.ToLower();

            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            if( await _repo.UserExist(user.UserName))
                return BadRequest("Username already exist");
            
            var userToCreate = new User
            {
                UserName = user.UserName
            };

            var rigisterUser = _repo.Register(userToCreate, user.Password);

            //later come back
            //return CreatedAtRoute("rout",object)

            return StatusCode(201); 
        }
    

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.Login(userForLoginDTO.UserName.ToLower(), userForLoginDTO.Password);

            if(userFromRepo == null)
                return Unauthorized();

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name, userForLoginDTO.UserName)
            };

            //? check
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

            var tokendescript = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials= creds,
                Expires= DateTime.Now.AddDays(1),

            };

            var tokenHander = new JwtSecurityTokenHandler();


            var token = tokenHander.CreateToken(tokendescript);
            string v = tokenHander.WriteToken(token);

            return Ok(new { //jason string
                token = v
            });   


        }
    
    }
}