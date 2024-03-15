using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class SprintRepository
    {
        private readonly AppDbContext _context;
        public SprintRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Sprint>> GetAllSprints()
        {
            if(_context.Sprints == null)
            {
                return new List<Sprint>();
            }
            return await _context.Sprints.ToListAsync();
        }
        public async Task<Sprint> GetSprintById(Guid id)
        {
            if(_context.Sprints == null)
            {
                return null;
            }
            if(_context.Sprints.Where(p=> p.Id == id) == null)
            {
                return null;
            }
            return await _context.Sprints.Where(p=> p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Sprint>> GetSprintsByProjectId(Guid projectId)
        {
            if(_context.Sprints == null)
            {
                return new List<Sprint>();
            }
            return await _context.Sprints.Where(p=> p.ProjectId == projectId).ToListAsync();
        }
        public async Task<Sprint> AddSprint(Sprint sprint)
        {
            var result = await _context.Sprints.AddAsync(sprint);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Sprint> DeleteSprint(Guid id)
        {
            var sprint = await _context.Sprints.Where(p=> p.Id == id).FirstOrDefaultAsync();
            if(sprint == null)
            {
                return null;
            }
            _context.Sprints.Remove(sprint);
            await _context.SaveChangesAsync();
            return sprint;
        }
        public async Task<Sprint> UpdateSprint(Sprint sprint)
        {
            var result = _context.Sprints.Update(sprint);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<List<Issue>> GetAllIssuesForSprint(Guid sprintId)
        {
            if(_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(i=> i.SprintId == sprintId).ToListAsync();
        }
    }
}
