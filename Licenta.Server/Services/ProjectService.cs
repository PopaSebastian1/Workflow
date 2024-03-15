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
        public async Task AddProject(Project project)
        {
            await _unitOfWork.ProjectsRepository.AddProject(project);
        }
        public async Task<Project?> DeleteProject(Guid id)
        {
            return await _unitOfWork.ProjectsRepository.DeleteProject(id);
        }
        public async Task UpdateProject(Project project)
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
    }
}
