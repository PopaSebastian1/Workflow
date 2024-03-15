using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class UserSkillService
    {
        private readonly UnitOfWork _unitOfWork;
        public UserSkillService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //GET ALL
        public async Task<List<Skill>> GetAllUserSkills(string email)
        {
            return await _unitOfWork.UserSkillRepository.GetSkillsAsync(email);
        }
        public async Task AddUserSkill(string email, Skill skill)
        {
            await _unitOfWork.UserSkillRepository.AddSkillUserAsync(email, skill);
        }
        public async Task DeleteUserSkill(string email, Guid skillId)
        {
            await _unitOfWork.UserSkillRepository.DeleteSkillAsync(email, skillId);
        }
        public async Task DeleteAllUserSkills(string email)
        {
            await _unitOfWork.UserSkillRepository.DeleteAllSkillsAsync(email);
        }
        public async Task UpdateSkill(string email, Skill skill)
        {
            await _unitOfWork.UserSkillRepository.UpdateSkillAsync(email, skill);
        }
    }

}
