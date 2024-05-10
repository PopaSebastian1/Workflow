using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueStatusController : Controller
    {
        private readonly IssueStatusService _issueStatusService;
        public IssueStatusController(IssueStatusService issueStatusService)
        {
            _issueStatusService = issueStatusService;
        }
        [HttpGet("GetAllIssueStatuses")]
        public async Task<IActionResult> GetAllIssueStatuses()
        {
            return Ok(await _issueStatusService.GetAllIssueStatuses());
        }
        [HttpGet("GetIssueStatusById")]
        public async Task<IActionResult> GetIssueStatusById(int id)
        {
            return Ok(await _issueStatusService.GetIssueStatusById(id));
        }
        [HttpPost("AddIssueStatus")]
        public async Task<IActionResult> AddIssueStatus(IssueStatus issueStatus)
        {
            return Ok(await _issueStatusService.AddIssueStatus(issueStatus));
        }
        [HttpDelete("DeleteIssueStatus")]
        public async Task<IActionResult> DeleteIssueStatus(int id)
        {
            return Ok(await _issueStatusService.DeleteIssueStatus(id));
        }
        [HttpPatch("UpdateIssueStatus")]
        public async Task<IActionResult> UpdateIssueStatus(IssueStatus issueStatus)
        {
            return Ok(await _issueStatusService.UpdateIssueStatus(issueStatus));
        }
        [HttpGet("GetIssueStatusByName")]
        public async Task<IActionResult> GetIssueStatusByName(string name)
        {
            return Ok(await _issueStatusService.GetIssueStatusByName(name));
        }
    }
}
