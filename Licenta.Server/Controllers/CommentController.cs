using Licenta.Server.DataLayer.Models;
using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllComments();
            return Ok(comments);
        }
        [HttpGet("GetComment/{id}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _commentService.GetComment(id);
            return Ok(comment);
        }
        [HttpPost("AddCommentToIssue/{issueId}")]
        public async Task<IActionResult> AddCommentToIssue(Comment comment, Guid issueId)
        {
            var newComment = await _commentService.AddCommentToIssue(comment, issueId);
            return Ok(newComment);
        }
        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var deletedComment = await _commentService.DeleteComment(id);
            return Ok(deletedComment);
        }
        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            var updatedComment = await _commentService.UpdateComment(comment);
            return Ok(updatedComment);
        }
        [HttpGet("GetCommentsByIssueId/{issueId}")]
        public async Task<IActionResult> GetCommentsByIssueId(Guid issueId)
        {
            var comments = await _commentService.GetCommentsByIssueId(issueId);
            return Ok(comments);
        }
        [HttpGet("GetCommentsByUserId/{title}")]
       public async Task<IActionResult> GetCommentsByUserId(string title)
        {
            var comments = await _commentService.GetCommentsByUserId(title);
            return Ok(comments);
        }
    }
}
