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
        [HttpGet("getUserById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok(await _userService.GetUserAsyncById(id));
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
        [HttpPost("JoinProject")]
        public async Task<IActionResult> JoinProject(string email, string key)
        {
            return Ok(await _userService.JoinProject(email, key));
        }
        [HttpGet("getUserProjects")]
        public async Task<IActionResult> GetUserProjects(string email)
        {
            return Ok(await _userService.GetAllUserProjects(email));
        }
        [HttpGet("getUserIssues")]
        public async Task<IActionResult> GetUserIssues(Guid Id)
        {
            return Ok(await _userService.GetAllUserIssues(Id));
        }
        [HttpGet("getAllUserIssuesByProject")]
        public async Task<IActionResult> GetAllUserIssuesByProject(Guid Id, string email)
        {
            return Ok(await _userService.GetAllUserIssuesByProjectId(Id, email));
        }

    }
}
