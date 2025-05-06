using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SimpleSocialApp.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;
        private readonly UserManager<AppUser> _userManager;


        public ReactionController(IPostService postService, 
            ICommentService commentService,
            IReactionService reactionService, 
            UserManager<AppUser> userManager)
        {
            _postService = postService;
            _commentService = commentService;
            _reactionService = reactionService;
            _userManager = userManager;
        }
       
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(string targetType, string targetId)
        {

            var user = await _userManager.GetUserAsync(User);
            int newLikeCount;

            if (targetType == "Post")
            {
                var post = await _postService.GetPostByIdAsync(targetId);
                if(post == null)
                {
                    return BadRequest();
                }

                var existingReaction = await _reactionService.SearchPostReactionAsync(targetId, user.Id);
                if (existingReaction != null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                }
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = user.Id,
                        PostId = targetId
                    };
                    await _reactionService.AddLikeAsync(reaction);
                }

                newLikeCount = await _postService.GetLikesCountAsync(targetId);
            }
            else if (targetType == "Comment")
            {
                var existingReaction = await _reactionService.SearchCommentReactionAsync(targetId, user.Id);
                if (existingReaction != null)
                {
                    await _reactionService.RemoveLikeAsync(existingReaction);
                }
                else
                {
                    var reaction = new Reaction
                    {
                        UserId = user.Id,
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
