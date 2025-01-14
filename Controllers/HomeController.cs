using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Services.Interfaces;
using System.Diagnostics;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;

        public HomeController(ILogger<HomeController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Post> friendsPosts = Enumerable.Empty<Post>();


            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(currentUserId))
            {
                friendsPosts = await _postService.GetAllUserFriendsPostsAsync(currentUserId);
            }

            var model = new HomeIndexViewModel
            {
                Posts = friendsPosts
            };

            return View(model);
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
