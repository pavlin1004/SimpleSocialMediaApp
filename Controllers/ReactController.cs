using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Implementations;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class ReactController : Controller
    {

        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;

        public ReactController(IPostService postService, ICommentService commentService, IReactionService reactionService)
        {
            _postService = postService;
            _commentService = commentService;
            _reactionService = reactionService;
        }

        [HttpPost]
        public async Task<IActionResult> Like(string targetType, string targetId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            if (targetType == "Post")
            {
                var post = await _postService.GetPostByIdAsync(targetId);
                if (post==null)
                    return BadRequest(new { Success = false, Message = "Invalid Post ID." });
                else
                {
                    var reaction = new Reaction 
                    {
                        UserId = currentUserId,
                        PostId = targetId,
                    };
                    await _reactionService.AddLikeAsync(reaction);
                    await _postService.AdjustCount(targetId, "Reaction", true);
                }
            }
            else if (targetType == "Comment")
            {
                var comment = await _commentService.GetCommentAsync(targetId);
                if (comment == null)
                    return BadRequest(new { Success = false, Message = "Invalid Comment ID." });
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = currentUserId,
                        CommentId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                }
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Invalid target type." });
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLike(string targetType, string targetId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            if (targetType == "Post")
            {
                var post = await _postService.GetPostByIdAsync(targetId);
                if (post == null)
                    return BadRequest(new { Success = false, Message = "Invalid Post ID." });
                else
                {
                    var reaction = await _reactionService.SearchPostReactionAsync(currentUserId, targetId);
                    if(reaction==null)
                    {
                        return NotFound();
                    }
                    await _reactionService.RemoveLikeAsync(reaction);
                    await _postService.AdjustCount(targetId, "Reaction", false);
                }
            }
            else if (targetType == "Comment")
            {
                var comment = await _commentService.GetCommentAsync(targetId);
                if (comment == null)
                    return BadRequest(new { Success = false, Message = "Invalid Comment ID." });
                else
                {
                    var reaction = await _reactionService.SearchCommentReactionAsync(currentUserId, targetId);
                    if (reaction == null)
                    {
                        return NotFound();
                    }
                    await _reactionService.RemoveLikeAsync(reaction);
                }
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Invalid target type." });
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }



    }
}
