using CsvHelper;
using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using System.Globalization;

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
        public async Task<List<Issue>> AddIssue(AddIssueDTO issue)
        {
            return  await _unitOfWork.IssueRepository.AddIssue(issue);
        }

        public async Task DeleteIssue(Guid id)
        {
            await _unitOfWork.IssueRepository.DeleteIssue(id);
        }
        public async Task DeleteIssueByTitle(string title)
        {
            await _unitOfWork.IssueRepository.DeleteIssueByTitle(title);
        }
        public async Task UpdateIssue(AddIssueDTO issue)
        {
            await _unitOfWork.IssueRepository.UpdateIssue(issue);
        }
        public async Task UpdateIssueTitle(Guid id, string newTitle)
        {
            await _unitOfWork.IssueRepository.UpdateIssueTitle(id, newTitle);
        }
        public async Task UpdateIssueDescription(Guid id, string newDescription)
        {
            await _unitOfWork.IssueRepository.UpdateIssueDescription(id, newDescription);
        }
        public async Task UpdateIssueDueDate(Guid id, DateTime newDueDate)
        {
            await _unitOfWork.IssueRepository.UpdateIssueDueDate(id, newDueDate);
        }
        public async Task UpdateIssueEstimatedTime(Guid id, float newEstimatedTime)
        {
            await _unitOfWork.IssueRepository.UpdateIssueEstimatedTime(id, newEstimatedTime);
        }
        public async Task UpdateIssueLoggedTime(Guid id, float newLoggedTime)
        {
            await _unitOfWork.IssueRepository.UpdateIssueLoggedTime(id, newLoggedTime);
        }
        public async Task AddIssueLoggedTime(Guid id, float newLoggedTime)
        {
            await _unitOfWork.IssueRepository.AddIssueLoggedTime(id, newLoggedTime);
        }
        public async Task RemoveIssueLoggedTime(Guid id, float newLoggedTime)
        {
            await _unitOfWork.IssueRepository.RemoveIssueLoggedTime(id, newLoggedTime);
        }
        public async Task ChangeAssigne(Guid id, Guid newAssigneeId)
        {
            await _unitOfWork.IssueRepository.ChangeAssigne(id, newAssigneeId);
        }
        public async Task ChangeReporter(Guid id, Guid newReporterId)
        {
            await _unitOfWork.IssueRepository.ChangeReporter(id, newReporterId);
        }
        public async Task ChangeIssueStatus(Guid id, int newStatus)
        {
            await _unitOfWork.IssueRepository.ChangeIssueStatus(id, newStatus);
        }
        public async Task ChangeIssueType(Guid id, int newType)
        {
            await _unitOfWork.IssueRepository.ChangeIssueType(id, newType);
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
        public async Task<List<Issue>> GetAllChildIssus(Guid parentId)
        {
            return await _unitOfWork.IssueRepository.GetAllChildIssues(parentId);
        }
        public async Task AddChildIssue(Guid parentId, Guid childId)
        {
            await _unitOfWork.IssueRepository.AddChildIssue(parentId, childId);
        }
        public async Task RemoveChildIssue(Guid parentId, Guid childId)
        {
            await _unitOfWork.IssueRepository.RemoveChildIssue(parentId, childId);
        }
        public async Task<Issue> GetParentIssue(Guid childId)
        {
            return await _unitOfWork.IssueRepository.GetParentIssue(childId);
        }
        public async Task<List<Issue>> GetAllChildIssueByParentId(Guid parentId)
        {
            return await _unitOfWork.IssueRepository.GetAllChildIssues(parentId);
        }

        public async Task<List<Issue>> GetIssuesByAssigneeId(Guid assignee)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByAssigneeId(assignee);
        }
        public async Task<List<Issue>> GetIssueByIssueStatusId(int status)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByIssueStatusId(status);
        }
        public async Task<List<Issue>> GetIssuesByProjectIdAndSprintId(Guid projectId, Guid sprintId)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByProjectIdAndSprintId(projectId, sprintId);
        }
        public async Task<List<Issue>> GetIssueByIssueTypId(int type)
        {
            return await _unitOfWork.IssueRepository.GetIssuesByIssueTypeId(type);
        }
    
        public async Task<string> ExportIssuesToCsv(List<Guid> ids, string filePath)
        {
            var issues = await _unitOfWork.IssueRepository.GetIssuesByIds(ids);
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(issues);
            }
            return filePath;
        }

    }
}
