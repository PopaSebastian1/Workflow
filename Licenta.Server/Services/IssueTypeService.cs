using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class IssueTypeService
    {
        private readonly UnitOfWork _unitOfWork;
        public IssueTypeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<IssueType>> GetAllIssueTypes()
        {
            return await _unitOfWork.IssueTypeRepository.GetAllIssueTypes();
        }
        public async Task<IssueType> GetIssueTypeById(int id)
        {
            return await _unitOfWork.IssueTypeRepository.GetIssueTypeById(id);
        }
        public async Task<IssueType> AddIssueType(IssueType issueType)
        {
            return await _unitOfWork.IssueTypeRepository.AddIssueType(issueType);
        }
        public async Task<IssueType> DeleteIssueType(int id)
        {
            return await _unitOfWork.IssueTypeRepository.DeleteIssueType(id);
        }
        public async Task<IssueType> UpdateIssueType(IssueType issueType)
        {
            return await _unitOfWork.IssueTypeRepository.UpdateIssueType(issueType);
        }
    }
}
