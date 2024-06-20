using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class IssueLabelRepository
    {
        private readonly AppDbContext _context;
        public IssueLabelRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<IssueLabel>> GetAllIssueLabels()
        {
            var issueLabels = await _context.Labels.ToListAsync();
            if (issueLabels == null)
            {
                return new List<IssueLabel>();
            }
            else
            {
                return issueLabels;
            }
        }
        public async Task<IssueLabel> GetIssueLabelById(int id)
        {
            var issueLabel = await _context.Labels.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueLabel == null)
            {
                return new IssueLabel();
            }
            else
            {
                return issueLabel;
            }
        }
        public async Task<IssueLabel> AddIssueLabel(IssueLabel issueLabel)
        {
            var result = await _context.Labels.AddAsync(issueLabel);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<IssueLabel> DeleteIssueLabel(int id)
        {
            var issueLabel = await _context.Labels.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueLabel == null)
            {
                return new IssueLabel();
            }
            else
            {
                _context.Labels.Remove(issueLabel);
                await _context.SaveChangesAsync();
                return issueLabel;
            }
        }
        public async Task<IssueLabel> UpdateIssueLabel(IssueLabel issueLabel)
        {
            var result = _context.Labels.Update(issueLabel);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task DeleteIssue(int id)
        {
            var issueLabel = await _context.Labels.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issueLabel == null)
            {
                return;
            }
            else
            {
                _context.Labels.Remove(issueLabel);
                await _context.SaveChangesAsync();
            }
        }
        
    }
}
