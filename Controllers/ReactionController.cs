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
            bool isLiked = false;

            if (targetType == "Post")
            {
                var post = await _postService.GetPostByIdAsync(targetId);
                if (post == null)
                    return BadRequest(new { Success = false, Message = "Invalid Post ID." });

                var existingReaction = await _reactionService.SearchPostReactionAsync(targetId, currentUserId);
                if (existingReaction !=  null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                    await _postService.ToggleLike(targetId, false);
                }
                else
                {                  
                    var reaction = new Reaction
                    {
                        UserId = currentUserId,
                        PostId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                    await _postService.ToggleLike(targetId, true);
                    isLiked = true;
                    
                }
                newLikeCount = post.LikesCount;
            }
            else if (targetType == "Comment")
            {
                var comment = await _commentService.GetCommentAsync(targetId);
                if (comment == null)
                    return BadRequest(new { Success = false, Message = "Invalid Comment ID." });

                var existingReaction = await _reactionService.SearchCommentReactionAsync(targetId,currentUserId);
                if (existingReaction != null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                    await _commentService.ToggleLike(targetId, false);
                }
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = currentUserId,
                        CommentId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                    await _commentService.ToggleLike(targetId, true);
                    isLiked = true;
                }
                newLikeCount = comment.LikesCount;
            }
            else
            {
                return Json(new { success = false, message = "Invalid target type" });
            }

            return Json(new { success = true, newLikeCount, isLiked });
        }
    }
}
