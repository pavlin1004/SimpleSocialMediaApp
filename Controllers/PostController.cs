using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services;

namespace SimpleSocialApp.Controllers
{
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

            //var friendships = await _userService.
            return  View();
        }
    }
}
