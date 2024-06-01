using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Composition.Convention;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            return Ok(projects);
        }
        [HttpGet("GetProjectById")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        [HttpPost("AddProject")]
        public async Task<IActionResult> AddProject(CreateProjectDTO project, Guid ownerId)
        {
            await _projectService.AddProject(project, ownerId);
            return Ok();
        }
        [HttpDelete("DeleteProject")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _projectService.DeleteProject(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        [HttpPut("UpdateProject")]
        public async Task<IActionResult> UpdateProject(AddProjectDto
            project)
        {
            await _projectService.UpdateProject(project);
            return Ok();
        }
        [HttpGet("GetAllProjectsForOwner")]
        public async Task<IActionResult> GetAllProjectsForOwner(string email)
        {
            var projects = await _projectService.GetAllProjectsForOwner(email);
            return Ok(projects);
        }
        [HttpPut("AddUserToProject")]
        public async Task<IActionResult> AddUserToProject(Guid projectId, string email)
        {
            await _projectService.AddUserToProject(projectId, email);
            return Ok();
        }
        [HttpPut("RemoveUserFromProject")]
        public async Task<IActionResult> RemoveUserFromProject(Guid projectId, string email)
        {
            await _projectService.RemoveUserFromProject(projectId, email);
            return Ok();
        }
        [HttpGet("GetAllIssuesForProject")]
        public async Task<IActionResult> GetAllIssuesForProject(Guid id)
        {
            var issues = await _projectService.GetAllIssuesForProject(id);
            return Ok(issues);
        }
        [HttpGet("GetAllSprintsForProject")]
        public async Task<IActionResult> GetAllSprintsForProject(Guid id)
        {
            var sprints = await _projectService.GetAllSprintsForProject(id);
            return Ok(sprints);
        }
        [HttpGet("GetAllUsersForProject")]
        public async Task<IActionResult> GetAllUsersForProject(Guid id)
        {
            var users = await _projectService.GetAllUsersForProject(id);
            return Ok(users);
        }
        [HttpGet("GetAllProjectsForMemeber")]
        public async Task<IActionResult> GetAllProjectsForMemeber(string email)
        {
            var projects = await _projectService.GetAllProjectsForMemeber(email);
            return Ok(projects);
        }
        [HttpPost("AddNewIssueStatusToProject")]
        public async Task<IActionResult> AddNewIssueStatusToProject(Guid projectId, IssueStatus status)
        {
            await _projectService.AddNewIssueStatusToProject(projectId, status);
            return Ok();
        }
        [HttpPost("AddIssueStatusToProject")]
        public async Task<IActionResult> AddIssueStatusToProject(Guid projectId, int issueStatusId)
        {
            await _projectService.AddIssueStatusToProject(projectId, issueStatusId);
            return Ok();
        }
        [HttpGet("GetIssueStatusFromProject")]
        public async Task<IActionResult> GetIssueStatusFromProject(Guid projectId)
        {
            var issueStatus = await _projectService.GetIssueStatusFromProject(projectId);
            return Ok(issueStatus);
        }
        [HttpGet("GetAllAdministratorsForProject")]
        public async Task<IActionResult> GetAllAdministratorsForProject(Guid projectId)
        {
            var administrators = await _projectService.GetAllAdministratorsForProject(projectId);
            return Ok(administrators);
        }
        [HttpPost("AddAdministratorToProject")]
        public async Task<IActionResult> AddAdministratorToProject(Guid projectId, string email)
        {
            await _projectService.AddAdministratorToProject(projectId, email);
            return Ok();
        }
        [HttpDelete("RemoveAdministratorFromProject")]
        public async Task<IActionResult> RemoveAdministratorFromProject(Guid projectId, string email)
        {
            await _projectService.RemoveAdministratorFromProject(projectId, email);
            return Ok();
        }
        [HttpDelete("RemoveSprintFromProject")]
        public async Task<IActionResult> RemoveSprintFromProject(Guid projectId, Guid sprintId)
        {
            await _projectService.RemoveSprintFromProject(projectId, sprintId);
            return Ok();
        }
    }
}
