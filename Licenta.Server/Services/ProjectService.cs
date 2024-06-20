using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class ProjectService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProjectService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Project>> GetAllProjects()
        {
            return await _unitOfWork.ProjectsRepository.GetAllProjects();
        }
        public async Task<Project> GetProjectById(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.GetProjectById(id);
        }
        public async Task AddProject(CreateProjectDTO project, Guid ownerId)
        {
            await _unitOfWork.ProjectsRepository.AddProject(project, ownerId);
        }
        public async Task<Project?> DeleteProject(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.DeleteProject(id);
        }
        public async Task UpdateProject(AddProjectDto project)
        {
            await _unitOfWork.ProjectsRepository.UpdateProject(project);
        }
        public async Task<List<Project>> GetAllProjectsForOwner(string email)
        {
            return await _unitOfWork.ProjectsRepository.GetAllProjectsForOwner(email);
        }
        public async Task<List<Issue>> GetAllIssuesForProject(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.GetAllIssuesForProject(id);
        }
        public async Task<List<Sprint>> GetAllSprintsForProject(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.GetAllSprintsForProject(id);
        }
        public async Task<List<User>> GetAllUsersForProject(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.GetAllUsersForProject(id);
        }
        public async Task AddUserToProject(Guid projectId, string email)
        {
            await _unitOfWork.ProjectsRepository.AddMemberToProject(email, projectId);
        }
        public async Task RemoveUserFromProject(Guid projectId, string email)
        {
            await _unitOfWork.ProjectsRepository.RemoveMemberFromProject(email, projectId);
        }
        public async Task<List<Project>> GetAllProjectsForMemeber(string email)
        {
            return await _unitOfWork.ProjectsRepository.GetAllProjectsForMemeber(email);
        }
        public async Task AddNewIssueStatusToProject(Guid projectId, IssueStatus status)
        {
            await _unitOfWork.ProjectsRepository.AddNewIssueStatusToProject(projectId, status);
        }
        public async Task AddIssueStatusToProject(Guid projectId, int issueStatusId)
        {
            await _unitOfWork.ProjectsRepository.AddIssueStatusToProject(projectId, issueStatusId);
        }
        public async Task<List<IssueStatus>> GetIssueStatusFromProject(Guid projectId)
        {
            return await _unitOfWork.ProjectsRepository.GetIssueStatusFromProject(projectId);
        }
        public async Task RemoveIssueStatusFromProject(Guid projectId, int issueStatusId)
        {
            await _unitOfWork.ProjectsRepository.RemoveIssueStatusFromProject(projectId, issueStatusId);
        }
        public async Task<List<User>> GetAllAdministratorsForProject(Guid projectId)
        {
            return await _unitOfWork.ProjectsRepository.GetAllAdministratorsForProject(projectId);
        }
        public async Task AddAdministratorToProject(Guid projectId, string email)
        {
            await _unitOfWork.ProjectsRepository.AddAdministratorToProject(projectId, email);
        }
        public async Task RemoveAdministratorFromProject(Guid projectId, string email)
        {
            await _unitOfWork.ProjectsRepository.RemoveAdministratorFromProject(projectId, email);
        }
        public async Task RemoveSprintFromProject(Guid projectId, Guid sprintId)
        {
            await _unitOfWork.ProjectsRepository.RemoveSprintFromProject(projectId, sprintId);
        }
        public async Task AddIssueLabel(Guid projectId, IssueLabel label)
        {
            await _unitOfWork.ProjectsRepository.AddIssueLabel(projectId, label);
        }
        public async Task  RemoveIssueLabel(Guid projectId, int labelId)
        {
            await _unitOfWork.ProjectsRepository.RemoveIssueLabel(projectId, labelId);
        }
        public async Task<List<IssueLabel>> GetIssueLabelsFromProject(Guid projectId)
        {
            return await _unitOfWork.ProjectsRepository.GetAllLabelsForProject(projectId);
        }
        public async Task RemoveIssueFromProject(Guid projectId, Guid issueId)
        {
            await _unitOfWork.ProjectsRepository.RemoveIssueFromProject(projectId, issueId);
        }
    } 
}
