using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, IPostService postService)
        {
            _commentService = commentService;
            _postService = postService;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("PostDetails", "Post", new { postId = inputModel.PostId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var comment = new Comment
            {
                UserId = userId,
                PostId = inputModel.PostId,
                Content = inputModel.Content,
                CreatedOnDate = DateTime.Now
            };

            await _commentService.CreateCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { postId = comment.PostId });
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            if (comment == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != currentUserId)
            {
                return Unauthorized(); // Only allow the owner to edit
            }

            var viewModel = new EditCommentViewModel
            {
                CommentId = comment.Id,
                Content = comment.Content
            };

            return View(viewModel);
        }

      
        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentViewModel model)
        {
            if (!ModelState.IsValid)
            { 
                return View(model); 
            }

            var comment = await _commentService.GetCommentAsync(model.CommentId);
            if (comment == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != currentUserId)
            {
                return Unauthorized(); 
            }

     
            comment.Content = model.Content;

     
            await _commentService.UpdateCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { postId = comment.PostId });
        }
        // Delete Comment (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(string commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            if (comment == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != currentUserId)
            {
                return Unauthorized(); // Only allow the owner to delete
            }

            await _commentService.DeleteCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { postId = comment.PostId });
        }



    }
}

