using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.api.Data;
using DatingApp.api.DTOS;
using DatingApp.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.api.Controllers {
    [Route ("api/[controller]")]
    [ApiController] //help input validation
    [AllowAnonymous] // means not need Anonymous
    public class AuthController : ControllerBase {
        //private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController (IConfiguration configuration, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._mapper = mapper;
            //this._repo = repository;
            this._configuration = configuration;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] UserForRegisterDTO user) {
            //[FromBody] let mvc core know where user come from
            //user.UserName = user.UserName.ToLower ();

            var userToCreate = _mapper.Map<User> (user);

            var result = await _userManager.CreateAsync (userToCreate,user.Password);

            var userToReturn = _mapper.Map<UserForDetailedDTO> (userToCreate);
            //var rigisterUser = _repo.Register (userToCreate, user.Password);

            //name set to method
            //Not really go to GetUser Method in Users Control, but return head will set url as GetUser path
            if (result.Succeeded) {
                return CreatedAtRoute ("GetUser", new { Controller = "Users", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest (result.Errors);
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login (UserForLoginDTO userForLoginDTO) {
            //  try use global handler in StartUp.cs
            //   {
            //throw new Exception("server is not working");

            //var userFromRepo = await _repo.Login (userForLoginDTO.UserName.ToLower (), userForLoginDTO.Password);

            var user = await _userManager.FindByNameAsync (userForLoginDTO.UserName);
            var result = await _signInManager.CheckPasswordSignInAsync (user, userForLoginDTO.Password, false);

            if (result.Succeeded) {
                var returnuser = _mapper.Map<UserForDetailedDTO> (user);

                return Ok (new { //jason string
                    token = createToken (user).Result,
                        user = returnuser
                });
            }

            return Unauthorized ();

            //if (userFromRepo == null)
            // return Unauthorized ();

            //  }
            // catch(Exception e)
            // {
            // return BadRequest("handler1: server doesn't work");
            // }   

        }

        private async Task<string> createToken (User user) {
            var claims = new List<Claim>{
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Name, user.UserName)
                //new Claim(ClaimTypes.r)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach( var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //? check
            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_configuration.GetSection ("AppSettings:token").Value));

            var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256Signature);

            var tokendescript = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays (1),

            };

            var tokenHander = new JwtSecurityTokenHandler ();

            var token = tokenHander.CreateToken (tokendescript);

            return tokenHander.WriteToken (token);

        }
    }
}