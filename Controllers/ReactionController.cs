using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;
using SimpleSocialApp.Data.Models;
using Microsoft.Extensions.Hosting;

namespace SimpleSocialApp.Controllers
{ 
    public class ReactionController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;

        public ReactionController(IPostService postService, ICommentService commentService, IReactionService reactionService)
        {
            _postService = postService;
            _commentService = commentService;
            _reactionService = reactionService;
        }
       
        [HttpPost]
        public async Task<IActionResult> ToggleLike(string targetType, string targetId)
        {
           
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            int newLikeCount = 0;

            if (targetType == "Post")
            {
                var post = await _postService.GetPostByIdAsync(targetId);
                if(post == null)
                {
                    return BadRequest();
                }

                var existingReaction = await _reactionService.SearchPostReactionAsync(targetId, currentUserId);
                if (existingReaction != null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                }
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = currentUserId,
                        PostId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                }

                newLikeCount = await _postService.GetLikesCountAsync(targetId);
            }
            else if (targetType == "Comment")
            {

                var comment = await _commentService.GetCommentAsync(targetId);

                var existingReaction = await _reactionService.SearchCommentReactionAsync(targetId, currentUserId);
                if (existingReaction != null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                }
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = currentUserId,
                        CommentId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                }

                newLikeCount = await _commentService.GetLikesCountAsync(targetId);
            }
            else
            {
                return Json(new { success = false, message = "Invalid target type" });
            }

            return Json(new { success = true, newLikeCount});
        }
    }
}
