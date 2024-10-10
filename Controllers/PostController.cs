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
using SimpleSocialApp.Services;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IUserService userService,IPostService postService) 
        {
                _postService = postService;
                _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var posts = await _postService.GetAllPostsAsync();

                return View(posts);
            }
            return  View();
        }

        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, List<Data.Models.Media> MediaFiles)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    post.UserId = userId;
                    post.PostedOn = DateTime.UtcNow;

                    if (MediaFiles != null && MediaFiles.Count > 0)
                    {
                        foreach (var file in MediaFiles)
                        {                         
                                post.Media.Add(file);                         
                        }
                    }

                    await _postService.AddPostAsync(post);
                    return RedirectToAction("Index", "Post");
                }
            }

            return View(post);
        }

        public IActionResult Edit(string id)
        {
            var post = _postService.GetPostByIdAsync(id);
            if(post==null)
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
                return RedirectToAction("Post","Index"); 
            }

            return View(post); 
        }
      
    }
}
