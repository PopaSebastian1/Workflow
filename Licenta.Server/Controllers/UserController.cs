using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Licenta.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
           return Ok(await _userService.GetAllAsync());
        }
        [HttpPost("addUserSkill")]
        public async Task<IActionResult> AddUserSkill(string email, Skill skill)
        {
            return Ok(await _userService.AddUserSkillAsync(email, skill));
        }
        [HttpGet("getUser/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            return Ok(await _userService.GetUserAsync(email));
        }
        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser(string email, string firstName, string lastName)
        {
            return Ok(await _userService.UpdateUserAsync(email, firstName, lastName));
        }
        
    }
}
