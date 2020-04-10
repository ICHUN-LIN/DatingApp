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
using AutoMapper;

namespace DatingApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //help input validation
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repository;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDTO user)
        {
            //[FromBody] let mvc core know where user come from
            user.UserName = user.UserName.ToLower();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repo.UserExist(user.UserName))
                return BadRequest("Username already exist");

            var userToCreate = _mapper.Map<User>(user);

            var rigisterUser = _repo.Register(userToCreate, user.Password);

            //later come back
            //return CreatedAtRoute("rout",object)

            //name set to method
            //Not really go to GetUser Method in Users Control, but return head will set url as GetUser path
            return CreatedAtRoute("GetUser",new { Controller="Users", id = rigisterUser.Id }, rigisterUser);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            //  try use global handler in StartUp.cs
            //   {
            //throw new Exception("server is not working");

            var userFromRepo = await _repo.Login(userForLoginDTO.UserName.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var returnuser = _mapper.Map<UserForDetailedDTO>(userFromRepo);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name, userForLoginDTO.UserName)
                //new Claim(ClaimTypes.r)
            };

            //? check
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokendescript = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(1),

            };

            var tokenHander = new JwtSecurityTokenHandler();


            var token = tokenHander.CreateToken(tokendescript);
            string v = tokenHander.WriteToken(token);

            return Ok(new
            { //jason string
                token = v,
                user = returnuser
            });
            //  }
            // catch(Exception e)
            // {
            // return BadRequest("handler1: server doesn't work");
            // }   


        }

    }
}