using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class IssueService
    {
        private readonly UnitOfWork _unitOfWork;
        public IssueService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Issue>> GetAllIssues()
        {
            return await _unitOfWork.IssueRepository.GetAllIssues();
        }
        public async Task<Issue> GetIssueById(Guid id)
        {
            return await _unitOfWork.IssueRepository.GetIssueById(id);
        }
        public async Task<Issue> AddIssue(AddIssueDTO issue)
        {
            return await _unitOfWork.IssueRepository.AddIssue(issue);
        }
        public async Task DeleteIssue(Guid id)
        {
             await _unitOfWork.IssueRepository.DeleteIssue(id);
        }
        public async Task UpdateIssue(AddIssueDTO issue)
        {
            await _unitOfWork.IssueRepository.UpdateIssue(issue);
        }
        public async Task<List<Issue>> GetIssuesByProjectId(Guid projectId)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByProjectId(projectId);
        }
        public async Task<List<Issue>> GetIssuesBySprintId(Guid sprintId)
        {
            return await _unitOfWork.IssueRepository.GetIssuesBySprintId(sprintId);
        }
        public async Task<List<Issue>> GetIssuesByUserId(Guid userId)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByAssigneeId(userId);
        }
        public async Task<List<Issue>> GetIssueByReporterId(Guid userId)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByReporterId(userId);
        }

    }
}
