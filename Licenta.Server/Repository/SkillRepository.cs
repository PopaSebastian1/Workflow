using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class SkillRepository
    {
        private readonly AppDbContext _context;
        public SkillRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Skill>> GetAll()
        {
            if(_context.Skills == null)
            {
                return new List<Skill>();
            }
            return await _context.Skills.AsQueryable().ToListAsync();
        }
        public async Task AddSkill(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }
        public async Task<Skill?> GetSkill(Guid id)
        {
            return await _context.Skills.FindAsync(id);
        }
        public async Task<Skill?> DeleteSkill(Guid id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }
            _context.Skills.Remove(skill);
            return skill;
        }
        public async Task UpdateSkill(Skill skill)
        {
            _context.Entry(skill).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
