using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Licenta.Server.Services;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : Controller
    {
        private readonly SkillService _skillService;
        public SkillsController(SkillService skillService)
        {
            _skillService = skillService;
        }
        [HttpGet("getAllSkills")]
        public async Task<ActionResult<List<Skill>>> GetAllSkills()
        {
            return await _skillService.GetAllSkills();
        }
        [HttpDelete("deleteSkill/{id}")]
        public async Task<ActionResult<Skill>> DeleteSkill(Guid id)
        {   var skill = await _skillService.GetSkill(id);
            if (skill == null)
            {
                return NotFound();
            }
            var skillToDelete = await _skillService.DeleteSkill(id);
            return skillToDelete;
        }
        [HttpGet("getSkill/{id}")]
        public async Task<ActionResult<Skill>> GetSkill(Guid id)
        {
            var skill = await _skillService.GetSkill(id);
            if (skill == null)
            {
                return NotFound();
            }
            return skill;
        }
        [HttpPost("addSkill")]
        public async Task<ActionResult<Skill>> AddSkill(Skill skill)
        {
            await _skillService.AddSkill(skill);
            return CreatedAtAction("GetSkill", new { id = skill.Id }, skill);
        }
        [HttpPost("updateSkill")]
        public async Task<ActionResult<Skill>> UpdateSkill(Skill skill)
        {
            await _skillService.UpdateSkill(skill);
            return CreatedAtAction("GetSkill", new { id = skill.Id }, skill);
        }
    }
}
