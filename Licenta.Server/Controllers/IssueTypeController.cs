using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueTypeController : Controller
    {
        private readonly IssueTypeService _issueTypeService;
        public IssueTypeController(IssueTypeService issueTypeService)
        {
            _issueTypeService = issueTypeService;
        }
        [HttpGet("GetAllIssueTypes")]
        public async Task<IActionResult> GetAllIssueTypes()
        {
            var result = await _issueTypeService.GetAllIssueTypes();
            return Ok(result);
        }
        [HttpGet("GetIssueTypeById")]
        public async Task<IActionResult> GetIssueTypeById(int id)
        {
            var result = await _issueTypeService.GetIssueTypeById(id);
            return Ok(result);
        }
        [HttpPost("AddIssueType")]
        public async Task<IActionResult> AddIssueType([FromBody] IssueType issueType)
        {
            var result = await _issueTypeService.AddIssueType(issueType);
            return Ok(result);
        }
        [HttpDelete("DeleteIssueType")]
        public async Task<IActionResult> DeleteIssueType(int id)
        {
            var result = await _issueTypeService.DeleteIssueType(id);
            return Ok(result);
        }
        [HttpPut("UpdateIssueType")]
        public async Task<IActionResult> UpdateIssueType([FromBody] IssueType issueType)
        {
            var result = await _issueTypeService.UpdateIssueType(issueType);
            return Ok(result);
        }
    }
}
