using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class IssueTypeRepository
    {
        private readonly AppDbContext _context;
        public IssueTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<IssueType>> GetAllIssueTypes()
        {
            var issueTypes = await _context.IssueTypes.ToListAsync();
            if (issueTypes == null)
            {
                return new List<IssueType>();
            }
            else
            {
                return issueTypes;
            }
        }
        public async Task<IssueType> GetIssueTypeById(int id)
        {
            var issueType = await _context.IssueTypes.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueType == null)
            {
                return new IssueType();
            }
            else
            {
                return issueType;
            }
        }
        public async Task<IssueType> AddIssueType(IssueType issueType)
        {
            var result = await _context.IssueTypes.AddAsync(issueType);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IssueType> DeleteIssueType(int id)
        {
            var issueType = await _context.IssueTypes.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueType == null)
            {
                return new IssueType();
            }
            else
            {
                _context.IssueTypes.Remove(issueType);
                await _context.SaveChangesAsync();
                return issueType;
            }
        }
        public async Task<IssueType> UpdateIssueType(IssueType issueType)
        {
            var result = _context.IssueTypes.Update(issueType);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        //public async Task<List<IssueType>> GetIssueTypesByProjectId(Guid projectId)
        //{
        //    var issueTypes = await _context.IssueTypes.Where(p => p. == projectId).ToListAsync();
        //    if (issueTypes == null)
        //    {
        //        return new List<IssueType>();
        //    }
        //    else
        //    {
        //        return issueTypes;
        //    }
        //}
    }
}
