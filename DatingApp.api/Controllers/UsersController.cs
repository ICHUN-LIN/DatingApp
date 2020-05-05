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
using DatingApp.api.Helpers;
using DatingApp.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.api.Controllers {
    [ServiceFilter (typeof (LogUserActivity))]
   // [Authorize]
    [Route ("api/[controller]")]
    [ApiController] //help input validation
    public class UsersController : ControllerBase {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        public UsersController (IDatingRepository datingRepository, IMapper mapper) {
            _mapper = mapper;
            _datingRepository = datingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Users ([FromQuery] UserParams param) {
            var id = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var currentUser = await this._datingRepository.GetUser (id);
            param.UserID = id;
            if (String.IsNullOrEmpty (param.Gender)) {
                param.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _datingRepository.GetUsers (param);
            var userToreturn = _mapper.Map<IEnumerable<UserForListDto>> (users);
            Response.AddPagination (users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok (userToreturn);
        }

        [HttpGet ("{id}", Name = "GetUser")]
        public async Task<IActionResult> Users (int id) {
            var user = await _datingRepository.GetUser (id);
            var userDto = _mapper.Map<UserForDetailedDTO> (user);
            return Ok (userDto);
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> UpdateUser (int id, UserForUpdateDTO userForUpdateDTO) {
            //if now user id != modify user id ==> no right to change
            if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            var userFromRep = await _datingRepository.GetUser (id);

            _mapper.Map (userForUpdateDTO, userFromRep);

            if (await _datingRepository.SaveAll ())
                return NoContent ();

            throw new Exception ($"User {id} is failed on Save");

        }

        [HttpPost ("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser (int id, int recipientId) {
            if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            var like = await _datingRepository.GetLike (id, recipientId);

            if (like != null)
                return BadRequest ("you already like this user");

            if (await _datingRepository.GetUser (recipientId) == null)
                return NotFound ();
            
            Like addLink = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _datingRepository.Add<Like>(addLink);

            if( await _datingRepository.SaveAll())
                return Ok();

            return BadRequest("fail to like user");

        }
    }
}