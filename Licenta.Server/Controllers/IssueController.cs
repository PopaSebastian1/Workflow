using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IssueController : Controller
    {
        private readonly IssueService _issueService;
        public IssueController(IssueService issueService)
        {
            _issueService = issueService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _issueService.GetAllIssues());
        }
        [HttpGet("GetIssue/{id}")]
        public async Task<IActionResult> GetIssue(Guid id)
        {
            return Ok(await _issueService.GetIssueById(id));
        }
        [HttpGet("GetIssueByAssignee/{assignee}")]
        public async Task<IActionResult> GetIssueByAssignee(Guid assignee)
        {
            return Ok(await _issueService.GetIssuesByAssigneeId(assignee));
        }
        [HttpGet("GetIssueByReporter/{reporter}")]
        public async Task<IActionResult> GetIssueByReporter(Guid reporter)
        {
            return Ok(await _issueService.GetIssueByReporterId(reporter));
        }
        [HttpGet("GetIssueByProjectId/{projectId}")]
        public async Task<IActionResult> GetIssueByProject(Guid projectId)
        {
            return Ok(await _issueService.GetIssuesByProjectId(projectId));
        }
        [HttpGet("GetIssueBySprintId/{sprintId}")]
        public async Task<IActionResult> GetIssueBySprint(Guid sprintId)
        {
            return Ok(await _issueService.GetIssuesBySprintId(sprintId));
        }
        [HttpGet("GetIssueByIssueStatusId/{status}")]
        public async Task<IActionResult> GetIssueByIssueStatus(int status)
        {
            return Ok(await _issueService.GetIssueByIssueStatusId(status));
        }
        [HttpGet("GetIssueByIssueTypeId/{type}")]
        public async Task<IActionResult> GetIssueByIssueType(int type)
        {
            return Ok(await _issueService.GetIssueByIssueTypId(type));
        }
        [HttpPost("AddIssue")]
        public async Task<IActionResult> AddIssue(AddIssueDTO issue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            return Ok(await _issueService.AddIssue(issue));
        }
        [HttpPost("UpdateIssue")]
        public async Task<IActionResult> UpdateIssue(AddIssueDTO issue)
        {
            await _issueService.UpdateIssue(issue);
            return Ok();

        }
        [HttpPost("DeleteIssue/{id}")]
        public async Task<IActionResult> DeleteIssue(Guid id)
        {
            await _issueService.DeleteIssue(id);
            return Ok();
        }
        [HttpGet("GetIssuesByProjectIdAndSprintId/{projectId}/{sprintId}")]
        public async Task<IActionResult> GetIssuesByProjectIdAndSprintId(Guid projectId, Guid sprintId)
        {
            return Ok(await _issueService.GetIssuesByProjectIdAndSprintId(projectId, sprintId));
        }
        [HttpGet("GetIssuesByUserId/{userId}")]
        public async Task<IActionResult> GetIssuesByUserId(Guid userId)
        {
            return Ok(await _issueService.GetIssuesByUserId(userId));
        }
        [HttpPost("UpdateIssueTitle/{id}/{title}")]
        public async Task<IActionResult> UpdateIssueTitle(Guid id, string title)
        {
            await _issueService.UpdateIssueTitle(id, title);
            return Ok();
        }
        [HttpPost("UpdateIssueDescription/{id}/{description}")]
        public async Task<IActionResult> UpdateIssueDescription(Guid id, string description)
        {
            await _issueService.UpdateIssueDescription(id, description);
            return Ok();
        }
        [HttpPost("UpdateIssueDueDate/{id}/{dueDate}")]
        public async Task<IActionResult> UpdateIssueDueDate(Guid id, DateTime dueDate)
        {
            await _issueService.UpdateIssueDueDate(id, dueDate);
            return Ok();
        }
        [HttpPost("UpdateIssueEstimatedTime/{id}/{estimatedTime}")]
        public async Task<IActionResult> UpdateIssueEstimatedTime(Guid id, float estimatedTime)
        {
            await _issueService.UpdateIssueEstimatedTime(id, estimatedTime);
            return Ok();
        }
        [HttpPost("UpdateIssueLoggedTime/{id}/{loggedTime}")]
        public async Task<IActionResult> UpdateIssueLoggedTime(Guid id, float loggedTime)
        {
            await _issueService.UpdateIssueLoggedTime(id, loggedTime);
            return Ok();
        }
        [HttpPost("AddIssueLoggedTime/{id}/{loggedTime}")]
        public async Task<IActionResult> AddIssueLoggedTime(Guid id, float loggedTime)
        {
            await _issueService.AddIssueLoggedTime(id, loggedTime);
            return Ok();
        }
        [HttpPost("RemoveIssueLoggedTime/{id}/{loggedTime}")]
        public async Task<IActionResult> RemoveIssueLoggedTime(Guid id, float loggedTime)
        {
            await _issueService.RemoveIssueLoggedTime(id, loggedTime);
            return Ok();
        }
        [HttpPatch("ChangeAssigne/{id}/{assigneeId}")]
        public async Task<IActionResult> ChangeAssigne(Guid id, Guid assigneeId)
        {
            await _issueService.ChangeAssigne(id, assigneeId);
            return Ok();
        }
        [HttpPatch("ChangeReporter/{id}/{reporterId}")]
        public async Task<IActionResult> ChangeReporter(Guid id, Guid reporterId)
        {
            await _issueService.ChangeReporter(id, reporterId);
            return Ok();
        }
        [HttpPost("ChangeIssueStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeIssueStatus(Guid id, int status)
        {
            await _issueService.ChangeIssueStatus(id, status);
            return Ok();
        }
        [HttpPost("ChangeIssueType/{id}/{type}")]
        public async Task<IActionResult> ChangeIssueType(Guid id, int type)
        {
            await _issueService.ChangeIssueType(id, type);
            return Ok();
        }
        [HttpGet("GetAllChildIssues/{parentId}")]
        public async Task<IActionResult> GetAllChildIssues(Guid parentId)
        {
            return Ok(await _issueService.GetAllChildIssus(parentId));
        }
        [HttpPost("AddChildIssue/{parentId}/{childId}")]
        public async Task<IActionResult> AddChildIssue(Guid parentId, Guid childId)
        {
            await _issueService.AddChildIssue(parentId, childId);
            return Ok();
        }
        [HttpPatch("RemoveChildIssue/{parentId}/{childId}")]
        public async Task<IActionResult> RemoveChildIssue(Guid parentId, Guid childId)
        {
            await _issueService.RemoveChildIssue(parentId, childId);
            return Ok();
        }
        [HttpGet("GetParentIssue/{childId}")]
        public async Task<IActionResult> GetParentIssue(Guid childId)
        {
            return Ok(await _issueService.GetParentIssue(childId));
        }
        [HttpGet("GetChildIssues/{parentId}")]
        public async Task<IActionResult> GetChildIssues(Guid parentId)
        {
            return Ok(await _issueService.GetAllChildIssueByParentId(parentId));
        }
        [HttpPost("ExportIssuesToCsv")]
        public async Task<IActionResult> ExportIssuesToCsv([FromBody] List<Guid> ids)
        {
            var filePath = Path.GetTempFileName() + ".csv";
            await _issueService.ExportIssuesToCsv(ids, filePath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            System.IO.File.Delete(filePath); // dele
            memory.Position = 0;
            return File(memory, "text/csv", Path.GetFileName(filePath));
        }
    }
}
