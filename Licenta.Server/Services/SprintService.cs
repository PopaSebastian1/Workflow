using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Enum;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class SprintService
    {
        private readonly UnitOfWork _unitOfWork;
        public SprintService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Sprint>> GetAllSprints()
        {
            return await _unitOfWork.SprintRepository.GetAllSprints();
        }
        public async Task<Sprint> GetSprintById(Guid id)
        {
            return await _unitOfWork.SprintRepository.GetSprintById(id);
        }
        public async Task<Sprint> AddSprint(AddSprintDto sprint)
        {
            return await _unitOfWork.SprintRepository.AddSprint(sprint);
        }
        public async Task<Sprint> DeleteSprint(Guid id)
        {
            return await _unitOfWork.SprintRepository.DeleteSprint(id);
        }
        public async Task<Sprint> UpdateSprint(Sprint sprint)
        {
            return await _unitOfWork.SprintRepository.UpdateSprint(sprint);
        }
        public async Task<List<Sprint>> GetSprintsByProjectId(Guid projectId)
        {
            return await _unitOfWork.SprintRepository.GetSprintsByProjectId(projectId);
        }
        public async Task<List<Issue>> GetAllIssuesForSprint(Guid id)
        {
            return await _unitOfWork.SprintRepository.GetAllIssuesForSprint(id);
        }
        public async Task UpdateSprintStatus(Guid id, int status)
        {
            await _unitOfWork.SprintRepository.UpdateSprintStatus(id, status);
        }
    }
}
