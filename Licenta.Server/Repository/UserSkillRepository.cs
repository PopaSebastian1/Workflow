using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class UserSkillRepository
    {
        private readonly AppDbContext _context;
        public UserSkillRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddSkillUserAsync(string email, Skill skill)
        {
            var user = await _context.Users.Where(t => t.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var newUserSkill = new UserSkill()
            {
                UserId = user.Id,
                SkillId = skill.Id
            };

                //add the new user skill to the database
            _context.UserSkills.Add(newUserSkill);
            _context.SaveChanges();
        }
        public async Task<List<Skill>> GetSkillsAsync(string email)
        {
            var user = await _context.Users.Where(t => t.Email == email).FirstOrDefaultAsync();
            //find the user skills
            var userSkills = await _context.UserSkills.Where(us => us.UserId == user.Id).AsQueryable().ToListAsync();
            //only return the skills of the user
            List<Skill> skills = new List<Skill>();
            foreach(var userSkill in userSkills)
            {
                var skill = await _context.Skills.Where(t=>t.Id==userSkill.SkillId).FirstOrDefaultAsync();
                skills.Add(skill);
            }
            return skills;
        }
        public async Task DeleteSkillAsync(string email, Guid skillId)
        {
            var user = await _context.Users.Where(t => t.Email == email).FirstOrDefaultAsync();
            var userSkill = await _context.UserSkills.FindAsync(user.Id, skillId);
            if (userSkill == null)
            {
                throw new ArgumentNullException(nameof(userSkill));
            }
            _context.UserSkills.Remove(userSkill);
            _context.SaveChanges();
        }
        public async Task DeleteAllSkillsAsync(string email)
        {
            var user = await _context.Users.Where(t => t.Email == email).FirstOrDefaultAsync();
            var userSkills = await _context.UserSkills.Where(us => us.UserId == user.Id).ToListAsync();
            if (userSkills == null)
            {
                throw new ArgumentNullException(nameof(userSkills));
            }
            _context.UserSkills.RemoveRange(userSkills);
            _context.SaveChanges();
        }
        public async Task UpdateSkillAsync(string email, Skill skill)
        {
            var user = await _context.Users.Where(t => t.Email == email).FirstOrDefaultAsync();
            var userSkill = await _context.UserSkills.FindAsync(user.Id, skill.Id);
            if (userSkill == null)
            {
                throw new ArgumentNullException(nameof(userSkill));
            }
            userSkill.SkillId = skill.Id;
            _context.SaveChanges();
        }
    }
}
