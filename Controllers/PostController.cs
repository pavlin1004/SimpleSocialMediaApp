using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels;
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

        public PostController(IUserService userService, IPostService postService, ICloudinaryService cloudinaryService, IMediaService mediaService)
        {
            _postService = postService;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _mediaService = mediaService;
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
        public async Task<IActionResult> Create(PostInputModel model)
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
                    PostedOn = DateTime.UtcNow
                };

                if (!String.IsNullOrEmpty(model.Text))
                {
                    p.Text = model.Text;
                }
                var uploadedFiles = Request.Form.Files;
                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    foreach (var media in uploadedFiles)
                    {
                        var mediaUrl = await _cloudinaryService.UploadMediaFileAsync(media);
                        if(!String.IsNullOrEmpty(mediaUrl))
                        {
                            p.Media.Add(new Media { Url = mediaUrl});
                        }
                        else
                        {
                            // Log or throw an error if media URL is empty
                            Console.WriteLine("Media upload failed for file: " + media.FileName);
                        }
                    }            
                }
               
                await _postService.AddPostAsync(p);
                return RedirectToAction("Index", "Home");
            }       
            return View(model);
        }

        public IActionResult Edit(string id)
        {
            var post = _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                RedirectToAction("Post", "Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {

                post.PostedOn = DateTime.UtcNow;

                await _postService.UpdatePostAsync(post);
                return RedirectToAction("Post", "Index");
            }

            return View(post);
        }

    }
}
