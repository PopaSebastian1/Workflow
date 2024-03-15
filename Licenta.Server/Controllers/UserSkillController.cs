using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserSkillController : Controller
    {
        private readonly UserSkillService _userSkillService;
        public UserSkillController(UserSkillService userSkillService)
        {
            _userSkillService = userSkillService;
        }
        [HttpGet("getAllUserSkills/{email}")]
        public async Task<List<Skill>> GetAllUserSkills(string email)
        {
            return await _userSkillService.GetAllUserSkills(email);
        }
        [HttpPost("addUserSkill/{email}")]
        public async Task AddUserSkill(string email, Skill skill)
        {
            await _userSkillService.AddUserSkill(email, skill);
        }
        [HttpDelete("deleteUserSkill/{email}/{skillId}")]
        public async Task DeleteUserSkill(string email, Guid skillId)
        {
            await _userSkillService.DeleteUserSkill(email, skillId);
        }
        [HttpDelete("deleteAllUserSkills/{email}")]
        public async Task DeleteAllUserSkills(string email)
        {
            await _userSkillService.DeleteAllUserSkills(email);
        }
        [HttpPost("updateUserSkill/{email}")]
        public async Task UpdateUserSkill(string email, Skill skill)
        {
            await _userSkillService.UpdateSkill(email, skill);
        }
    }
}
