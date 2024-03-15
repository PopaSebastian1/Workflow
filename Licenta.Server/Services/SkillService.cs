using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class SkillService
    {
        private readonly UnitOfWork _unitOfWork;
        public SkillService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Skill>> GetAllSkills()
        {
            return await _unitOfWork.SkillRepository.GetAll();
        }
        public async Task AddSkill(Skill skill)
        {
            await _unitOfWork.SkillRepository.AddSkill(skill);
        }
        public async Task<Skill?> GetSkill(Guid id)
        {
            return await _unitOfWork.SkillRepository.GetSkill(id);
        }   
        public async Task<Skill?> DeleteSkill(Guid id)
        {
            return await _unitOfWork.SkillRepository.DeleteSkill(id);
        }
        public async Task UpdateSkill(Skill skill)
        {
            await _unitOfWork.SkillRepository.UpdateSkill(skill);
        }

    }
}
