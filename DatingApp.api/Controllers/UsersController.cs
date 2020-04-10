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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace DatingApp.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController] //help input validation
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        { 
            _mapper = mapper;
            _datingRepository = datingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _datingRepository.GetUsers();
            var userDtos = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Users(int id)
        {
            var user = await _datingRepository.GetUser(id);
            var userDto = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userDto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id,UserForUpdateDTO  userForUpdateDTO)
        {
            //if now user id != modify user id ==> no right to change
            if(id!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRep = await _datingRepository.GetUser(id);

            _mapper.Map(userForUpdateDTO,userFromRep);

            if(await _datingRepository.SaveAll())
                return NoContent();

            throw new Exception($"User {id} is failed on Save");

        }
    }
}