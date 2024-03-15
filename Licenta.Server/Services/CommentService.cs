using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;

namespace Licenta.Server.Services
{
    public class CommentService
    {
        private readonly UnitOfWork _unitOfWork;
        public CommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Comment>> GetAllComments()
        {
            return await _unitOfWork.CommentRepository.GetAllComments();
        }
        public async Task<Comment?> GetComment(Guid id)
        {
            return await _unitOfWork.CommentRepository.GetCommentById(id);
        }
        public async Task<Comment?> AddCommentToIssue(Comment comment, Guid issueId)
        {
            return await _unitOfWork.CommentRepository.AddCommentToIssue(comment, issueId);
        }
        public async Task<Comment?> DeleteComment(Guid id)
        {
            return await _unitOfWork.CommentRepository.DeleteComment(id);
        }
        public async Task<Comment?> UpdateComment(Comment comment)
        {
            return await _unitOfWork.CommentRepository.UpdateComment(comment);
        }
        public async Task<List<Comment>> GetCommentsByIssueId(Guid issueId)
        {
            return await _unitOfWork.CommentRepository.GetAllCommentsForIssue(issueId);
        }
        public async Task<List<Comment>> GetCommentsByUserId(string title)
        {
            return await _unitOfWork.CommentRepository.GetAllCommentsForIssueByName(title);
        }

    }
}
