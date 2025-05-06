using System.Collections.ObjectModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSociaMedialApp.Services.External.Interfaces;
using SimpleSociaMedialApp.Services.Functional.Interfaces;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMediaService _mediaService;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;

        public PostController( IPostService postService, ICloudinaryService cloudinaryService, IMediaService mediaService, ICommentService commentService, UserManager<AppUser> userManager)
        {
            _postService = postService;
            _cloudinaryService = cloudinaryService;
            _mediaService = mediaService;
            _commentService = commentService;
            _userManager = userManager;
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
                RedirectToAction("Index", "Home");
            }

            var user = await _userManager.GetUserAsync(User);

            Post p = new()
            {
                UserId = user.Id,
                CreatedDateTime = DateTime.UtcNow,
                Media = [],
            };

            if (!String.IsNullOrEmpty(model.Content))
            {
                p.Content = model.Content;
            }
            if (model.MediaFiles != null && model.MediaFiles.Count != 0)
            {
                foreach (var media in model.MediaFiles)
                {
                    var mediaData = await _cloudinaryService.UploadMediaFileAsync(media);
                    if (mediaData != null)

                    {
                        p.Media.Add(new Media
                        {
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string postId)
        {

            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null) return NotFound();

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null) return NotFound();

            var viewModel = new EditPostViewModel
            {
                PostId = post.Id,
                Content = post.Content ?? ""
            };
            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var post = await _postService.GetPostByIdAsync(model.PostId);
            if (post == null) return NotFound();
         
            post.Content = model.Content;

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
            var user = _userManager.GetUserAsync(User);

            foreach(var media in post.Media)
            {
                await _cloudinaryService.DeleteMediaAsync(media.PublicId);
            }

            var result = await _mediaService.RemoveMediaForPostAsync(post);
            if(result == false) return BadRequest();

            await _postService.DeletePostAsync(postId);

            return RedirectToAction("Index", "Home", new { userId = user.Id });
        }
    }
}
