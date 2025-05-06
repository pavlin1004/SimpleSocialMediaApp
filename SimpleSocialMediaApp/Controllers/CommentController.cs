using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels;
using SimpleSocialApp.Models.ViewModels;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentService commentService,UserManager<AppUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("PostDetails", "Post", new { postId = inputModel.PostId });
            }

            var user = await _userManager.GetUserAsync(User);
            var comment = new Comment
            {
                UserId = user.Id,
                PostId = inputModel.PostId,
                Content = inputModel.Content,
                CreatedDateTime = DateTime.Now
            };

            await _commentService.CreateCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { postId = comment.PostId });
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            if (comment == null) return NotFound();

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

            await _commentService.DeleteCommentAsync(comment);

            return RedirectToAction("Details", "Post", new { postId = comment.PostId });
        }



    }
}

