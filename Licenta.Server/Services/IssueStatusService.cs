using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class IssueStatusService
    {
        private readonly UnitOfWork _unitOfWork;
        public IssueStatusService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<IssueStatus>> GetAllIssueStatuses()
        {
            return await _unitOfWork.IssueStatusRepository.GetAllIssueStatuses();
        }
        public async Task<IssueStatus> GetIssueStatusById(int id)
        {
            return await _unitOfWork.IssueStatusRepository.GetIssueStatusById(id);
        }
        public async Task<IssueStatus> AddIssueStatus(IssueStatus issueStatus)
        {
            return await _unitOfWork.IssueStatusRepository.AddIssueStatus(issueStatus);
        }
        public async Task<IssueStatus> DeleteIssueStatus(int id)
        {
            return await _unitOfWork.IssueStatusRepository.DeleteIssueStatus(id);
        }
        public async Task<IssueStatus> UpdateIssueStatus(IssueStatus issueStatus)
        {
            return await _unitOfWork.IssueStatusRepository.UpdateIssueStatus(issueStatus);
        }
        public async Task<IssueStatus>GetIssueStatusByName(string name)
        {
            return await _unitOfWork.IssueStatusRepository.GetIssueStatusByName(name);
        }
    }
}
