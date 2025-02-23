using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Services.Implementations;
using SimpleSocialApp.Services.Interfaces;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFriendshipService _friendshipService;
        private readonly IPostService _postService;
        private readonly IMediaService _mediaService;
        private readonly ICloudinaryService _cloudinaryService;

        public UserController(IUserService userService, IFriendshipService friendshipService, IPostService postService, IMediaService mediaService, ICloudinaryService cloudinaryService)
        {
            _userService = userService;
            _friendshipService = friendshipService;
            _postService = postService;
            _mediaService = mediaService;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("Could not retrueve user ID");
            }
            // Fetch user data
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            // Fetch posts
            var posts = await _postService.GetAllUserPostsAsync(userId);

            // Fetch friendship status or default to "None"
            Friendship? friendshipStatus = null;
            friendshipStatus = await _friendshipService.CheckFriendship(currentUserId, userId);


            var viewModel = new AppUserViewModel
            {
                User = user,
                Posts = posts,
                FriendshipStatus = friendshipStatus, // Null if no relationship exists
                IsCurrentUser = (currentUserId == userId)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("Could not retrueve user ID");
            }

            if (currentUserId == userId) return BadRequest("You cannot send a friend request to yourself.");

            await _friendshipService.SendFriendshipRequestAsync(currentUserId, userId); // Use "Pending" as status
            return RedirectToAction("Profile", new { userId });
        }
        [HttpPost]
        public async Task<IActionResult> AcceptFriendshipRequest(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("Could not retrueve user ID");
            }

            if (currentUserId == userId) return BadRequest("You cannot send a friend request to yourself.");

            await _friendshipService.AcceptUserFriendshipAsync(currentUserId, userId);
            
            return RedirectToAction("Profile", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriendship(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("Could not retrueve user ID");
            }

            if (currentUserId == userId) return BadRequest("You cannot send a friend request to yourself.");

            await _friendshipService.RemoveUserFriendshipsAsync(currentUserId, userId);
            return RedirectToAction("Profile", new { userId });
        }

        // GET method to show the page to upload a profile picture
        [HttpGet]
        public IActionResult EditProfilePicture()
        {
            return View();
        }

        // POST method to handle media file upload and save the media URL to the user profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfilePicture(IFormFile mediaFile)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || mediaFile == null)
            {
                return BadRequest("User or file is missing");
            }

            // Validate the file type (image)
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!validImageTypes.Contains(mediaFile.ContentType.ToLower()))
            {
                return BadRequest("Invalid file type. Only image files are allowed.");
            }

            // Upload the file (using Cloudinary or your media service)
            var mediaUrl = await _cloudinaryService.UploadMediaFileAsync(mediaFile);
            if (string.IsNullOrEmpty(mediaUrl.Item1))
            {
                return BadRequest("Media upload failed");
            }

            // Update the user's profile with the new image
            var user = await _userService.GetUserByIdAsync(currentUserId);
            if (user == null)
            {
                return NotFound();
            }

            user.Media = new Media { Url = mediaUrl.Item1 };
            await _userService.UpdateAsync(user);

            // Redirect back to the profile
            return RedirectToAction("Profile", new { userId = currentUserId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string searchQuery)
        {
            // If no query is entered, you can either show an empty result or handle it
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index", "Home"); // Redirect to home or show a message
            }

            // Fetch users based on search query
            var users = await _userService.SearchUsersByNameAsync(searchQuery);

            return View(users); // Return the view with results
        }


    }
}

