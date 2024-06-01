using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SprintController : Controller
    {
       private readonly SprintService _sprintService;
        public SprintController(SprintService sprintService)
        {
            _sprintService = sprintService;
        }
        [HttpGet("GetAllSprints")]
        public async Task<IActionResult> GetAllSprints()
        {
            return Ok(await _sprintService.GetAllSprints());
        }
        [HttpGet("GetSprintById")]
        public async Task<IActionResult> GetSprintById(Guid id)
        {
            return Ok(await _sprintService.GetSprintById(id));
        }
        [HttpPost("AddSprint")]
        public async Task<IActionResult> AddSprint(AddSprintDto sprint)
        {
            return Ok(await _sprintService.AddSprint(sprint));
        }
        [HttpDelete("DeleteSprint")]
        public async Task<IActionResult> DeleteSprint(Guid id)
        {
            return Ok(await _sprintService.DeleteSprint(id));
        }
        [HttpPut("UpdateSprint")]
        public async Task<IActionResult> UpdateSprint(Sprint sprint)
        {
            return Ok(await _sprintService.UpdateSprint(sprint));
        }
        [HttpGet("GetSprintsByProjectId")]
        public async Task<IActionResult> GetSprintsByProjectId(Guid projectId)
        {
            return Ok(await _sprintService.GetSprintsByProjectId(projectId));
        }
        [HttpGet("GetAllIssuesForSprint")]
        public async Task<IActionResult> GetAllIssuesForSprint(Guid id)
        {
            return Ok(await _sprintService.GetAllIssuesForSprint(id));
        }
        [HttpPatch("UpdateSprintStatus")]
        public async Task<IActionResult> UpdateSprintStatus(Guid id, int status)
        {
            await _sprintService.UpdateSprintStatus(id, status);
            return Ok();
        }
    }
}
