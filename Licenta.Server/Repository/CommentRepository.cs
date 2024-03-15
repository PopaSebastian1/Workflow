using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class CommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllComments()
        {
            var comments = await _context.Comments.ToListAsync();
            if (comments == null)
            {
                return new List<Comment>();
            }
            else
            {
                return comments;
            }
        }
        public async Task<Comment> GetCommentById(Guid id)
        {
            var comment = await _context.Comments.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return new Comment();
            }
            else
            {
                return comment;
            }
        }
        public async Task<Comment> AddCommentToIssue(Comment comment, Guid issueId)
        {
            var issue = await _context.Issues.Where(p => p.Id == issueId).FirstOrDefaultAsync();
            if (issue == null)
            {
                return new Comment();
            }
            var result = await _context.Comments.AddAsync(comment);
            issue.Comments.Add(comment);
            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Comment> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (comment == null)
            {
                return new Comment();
            }
            else
            {
                var issue = _context.Issues.Where(p => p.Comments.Contains(comment)).FirstOrDefault();
                issue.Comments.Remove(comment);
                _context.Issues.Update(issue);
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
        }
        public async Task<Comment> UpdateComment(Comment comment)
        {
            var result = _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<List<Comment>> GetAllCommentsForIssue(Guid issueId)
        {
            var issue = await _context.Issues.Where(p => p.Id == issueId).FirstOrDefaultAsync();
            if (issue == null)
            {
                return new List<Comment>();
            }
            return issue.Comments;
        }
        public async Task<List<Comment>> GetAllCommentsForIssueByName(string title)
        {
            var issue = await _context.Issues.Where(p => p.Title == title).FirstOrDefaultAsync();
            if (issue == null)
            {
                return new List<Comment>();
            }
            return issue.Comments;
        }
    }
}
