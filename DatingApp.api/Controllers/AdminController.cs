using System.Linq;
using System.Threading.Tasks;
using DatingApp.api.Data;
using DatingApp.api.DTOS;
using DatingApp.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;

        public AdminController(DataContext dataContext, UserManager<User> userManager)
        {
            this._userManager = userManager;
            this._dataContext = dataContext;

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("UsersWithRoles")]
        public async System.Threading.Tasks.Task<IActionResult> GetUserWithRolesAsync()
        {
            var users = await this._dataContext.Users.OrderBy(x => x.UserName).Select(
                user => new
                {
                    id = user.Id,
                    UserName = user.UserName,
                    roles = (from userRole in
                    user.UserRoles
                             join role in _dataContext.Roles on
              userRole.RoleId equals role.Id
                             select role.Name).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);
            string[] stringuserRoles = userRoles.ToArray();

            var selectedRoles = roleEditDTO.roleNames;

            selectedRoles = selectedRoles?? new string[]{};

            var result = _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles)).Result;
            if(!result.Succeeded)
                return BadRequest("Fail to add to roles");
            
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded)
                return BadRequest("Fail to remove roles");
            
            return Ok(await _userManager.GetRolesAsync(user));
            
            //var result = _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        }

    }
}