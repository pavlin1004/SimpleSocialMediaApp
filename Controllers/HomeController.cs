using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSocialApp.Services.Interfaces;
using System.Diagnostics;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService; 
        public HomeController(ILogger<HomeController> logger, IPostService postService, ICommentService commentService)
        { 
            _logger = logger;
            _postService = postService;
            _commentService = commentService; 
        }
        [Authorize]
        public async Task<IActionResult> Index(int size = 5, int count = 0)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }
            var friendsPosts = await _postService.GetAllAsync(size, count);
            var postViewModels = new List<PostViewModel>();
            if (!(friendsPosts == null || friendsPosts.Count == 0))
            {
                foreach (var post in friendsPosts)
                {
                    //Factory class is optional
                    postViewModels.Add(new PostViewModel
                    {
                        Post = post,
                        Comments = await _commentService.GetAllPostComments(post.Id),
                        LikesCount = await _postService.GetLikesCountAsync(post.Id),
                        CommentsCount = await _postService.GetCommentsCountAsync(post.Id)
                    });
                }
            }
            if (count == 0)
            {
                return View(new HomeIndexViewModel { Posts = postViewModels });
            }
            else
            {
                //Unlimited scroll post loading
                return PartialView("Post/_PostPartial", postViewModels);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
