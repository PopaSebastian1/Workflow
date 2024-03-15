using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;
namespace Licenta.Server.Repository
{
    public class IssueStatusRepository
    {
        private readonly AppDbContext _context;
        public IssueStatusRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<IssueStatus>> GetAllIssueStatuses()
        {
            var issueStatuses = await _context.IssuesStatus.ToListAsync();
            if (issueStatuses == null)
            {
                return new List<IssueStatus>();
            }
            else
            {
                return issueStatuses;
            }
        }
        public async Task<IssueStatus> GetIssueStatusById(int id)
        {
            var issueStatus = await _context.IssuesStatus.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueStatus == null)
            {
                return new IssueStatus();
            }
            else
            {
                return issueStatus;
            }
        }
        public async Task<IssueStatus> AddIssueStatus(IssueStatus issueStatus)
        {
            var result = await _context.IssuesStatus.AddAsync(issueStatus);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IssueStatus> DeleteIssueStatus(int id)
        {
            var issueStatus = await _context.IssuesStatus.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueStatus == null)
            {
                return new IssueStatus();
            }
            else
            {
                _context.IssuesStatus.Remove(issueStatus);
                await _context.SaveChangesAsync();
                return issueStatus;
            }
        }
        public async Task<IssueStatus> UpdateIssueStatus(IssueStatus issueStatus)
        {
            var result = _context.IssuesStatus.Update(issueStatus);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IssueStatus> GetIssueStatusByName(string name)
        {
            var issueStatus = await _context.IssuesStatus.Where(p => p.Name == name).FirstOrDefaultAsync();
            if (issueStatus == null)
            {
                return new IssueStatus();
            }
            else
            {
                return issueStatus;
            }
        }
    }
}
