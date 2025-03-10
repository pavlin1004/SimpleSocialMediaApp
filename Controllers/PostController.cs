using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSocialApp.Services.Implementations;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMediaService _mediaService;
        private readonly IReactionService _reactionService;
        private readonly ICommentService _commentService;
        public PostController(IUserService userService, IPostService postService, ICloudinaryService cloudinaryService, IMediaService mediaService, IReactionService reactionService, ICommentService commentService)
        {
            _postService = postService;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _mediaService = mediaService;
            _reactionService = reactionService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var posts = await _postService.GetAllUserFriendsPostsAsync("a");

                return View(posts);
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Return the view with validation errors if any
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                Post p = new Post
                {
                    UserId = userId,
                    PostedOn = DateTime.UtcNow,
                    Media = new Collection<Media>(),
                    LikesCount = 0,
                    CommentCount = 0
                };

                if (!String.IsNullOrEmpty(model.Text))
                {
                    p.Text = model.Text;
                }
                foreach (var media in model.MediaFiles)
                {
                    var mediaData = await _cloudinaryService.UploadMediaFileAsync(media);
                    if (mediaData != null)
                        
                    {
                        p.Media.Add( new Media {
                                Url = mediaData[0],
                                PublicId = mediaData[1],
                                Type = mediaData[2] == "Image" ? MediaOptions.Image : MediaOptions.Video
                            });
                    }
                    else
                    {
                        // Log or throw an error if media URL is empty
                        Console.WriteLine("Media upload failed for file: " + media.FileName);
                    }
                }
                await _postService.AddPostAsync(p);
            }
            return RedirectToAction("Index", "Home");
        }       

        [HttpGet]
        public async Task<IActionResult> Details(string postId)
        {

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                throw new ArgumentNullException("");
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return BadRequest();
            }

            var comments = await _commentService.GetAllPostComments(postId);

            var viewModel = new PostViewModel
            {
                Post = post,
                Comments = comments,
                CommentsCount = await _postService.GetCommentsCountAsync(postId),
                LikesCount = await _postService.GetLikesCountAsync(postId)
            };

            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != currentUserId)
            {
                return Unauthorized(); 
            }  
            var viewModel = new EditPostViewModel
            {
                PostId = post.Id,
                Text = post.Text
            };
            return View(viewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var post = await _postService.GetPostByIdAsync(model.PostId);
            if (post == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != currentUserId)
            {
                return Unauthorized(); 
            }
         
            post.Text = model.Text;

            await _postService.UpdatePostAsync(post);

            return RedirectToAction("Details", "Post", new { postId = post.Id });
        }

       
        public async Task<IActionResult> Delete(string postId)
        {
            if (string.IsNullOrEmpty(postId))
            {
                return BadRequest("Invalid post ID.");
            }

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            if (post.UserId != currentUserId)
            {
                return Unauthorized(); // More appropriate than Unauthorized for permission issues
            }

            foreach(var media in post.Media)
            {
                await _cloudinaryService.DeleteMediaAsync(media.PublicId);
                await _mediaService.RemoveUserMediaAsync(media.Id);
            }
            // Perform deletion
            await _postService.DeletePostAsync(postId);

            // Redirect back to the home page or user profile (if applicable)
            return RedirectToAction("Index", "Home", new { userId = currentUserId });
        }
    }
}
