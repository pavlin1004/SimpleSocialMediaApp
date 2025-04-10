using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Mapping;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Models.ViewModels.AppUsers;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSocialApp.Services.Implementations;
using SimpleSocialApp.Services.Interfaces;
using SimpleSociaMedialApp.Models.ViewModels.AppUsers;
using System.Security.Claims;
using System.Transactions;

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
        private readonly IChatService _chatService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IFriendshipService friendshipService, IPostService postService, IMediaService mediaService, ICloudinaryService cloudinaryService, IChatService chatService, ICommentService commentService,IMapper mapper)
        {
            _userService = userService;
            _friendshipService = friendshipService;
            _postService = postService;
            _mediaService = mediaService;
            _cloudinaryService = cloudinaryService;
            _chatService = chatService;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userId, int size = 5, int count = 0)
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
            var posts = await _postService.GetAllUserPostsAsync(userId, size, count);

            var postViewModels = new List<PostViewModel>();

            foreach (var post in posts)
            {
                postViewModels.Add(new PostViewModel
                {
                    Post = post,
                    Comments = await _commentService.GetAllPostComments(post.Id),
                    LikesCount = await _postService.GetLikesCountAsync(post.Id),
                    CommentsCount = await _postService.GetCommentsCountAsync(post.Id)
                });
            }
            // Fetch friendship status or default to "None"
            Friendship? friendshipStatus = null;
            friendshipStatus = await _friendshipService.CheckFriendship(currentUserId, userId);


            var viewModel = new ProfileViewModel
            {
                User = user,
                Posts = postViewModels,
                FriendshipStatus = friendshipStatus, // Null if no relationship exists
                IsCurrentUser = (currentUserId == userId)
            };
            if (count == 0)
            {
                return View(viewModel);
            }
            else return PartialView("Post/_PostPartial", postViewModels);
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

            var currentUser = await _userService.GetUserByIdAsync(currentUserId);
            var friend = await _userService.GetUserByIdAsync(userId);
            if (currentUser != null && friend != null)
            {
                await _chatService.CreatePrivateChatAsync(currentUser, friend);
            } 
            await _friendshipService.AcceptUserFriendshipAsync(currentUserId, userId);
            return RedirectToAction("Profile", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriendship(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("Could not retrieve user ID");
            }

            if (currentUserId == userId) return BadRequest("You cannot send a friend request to yourself.");

            await _friendshipService.RemoveUserFriendshipsAsync(currentUserId, userId);
            return RedirectToAction("Profile", new { userId });
        }

        // GET method to show the page to upload a profile picture
        [HttpGet]
        public async Task<IActionResult> EditProfilePicture()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }
            var user = await _userService.GetUserByIdAsync(currentUserId); // Fetch the user object

            if (user == null)
            {
                return NotFound();
            }

            var currentProfilePicture = user.Media;
           
            return View(currentProfilePicture); // Pass the profile picture to the view (if any)
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfilePicture(IFormFile mediaFile)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null || mediaFile == null)
            {
                return BadRequest("User or file is missing");
            }
            var user = await _userService.GetUserByIdAsync(currentUserId);
            if(user==null)
            {
                return Unauthorized();
            }

            // Validate the file type (image)
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!validImageTypes.Contains(mediaFile.ContentType.ToLower()))
            {
                return BadRequest("Invalid file type. Only image files are allowed.");
            }
            var currentProfilePic = user.Media;
            if(currentProfilePic != null && currentProfilePic.PublicId!=null)
            {
                await _cloudinaryService.DeleteMediaAsync(currentProfilePic.PublicId);

            }
            // Upload the file (using Cloudinary or your media service)
            var mediaData = await _cloudinaryService.UploadMediaFileAsync(mediaFile);
            if (String.IsNullOrEmpty(mediaData[0]) || String.IsNullOrEmpty(mediaData[1]))
            {
                return BadRequest("Media upload failed");
            }

            if (user.Media == null)
            {
                await _userService.AddProfilePictureAsync(user, new Media { Url = mediaData[0], PublicId = mediaData[1] });
            }
            else
            {
                user.Media.Url = mediaData[0];
                user.Media.PublicId = mediaData[1];
                await _userService.UpdateAsync(user);
            }
            // Redirect back to the profile
            return RedirectToAction("Profile", new { userId = currentUserId });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string searchQuery)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(String.IsNullOrEmpty(currentUser))
            {
                return Unauthorized();
            }
            // If no query is entered, you can either show an empty result or handle it
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index", "Home"); // Redirect to home or show a message
            }

            // Fetch users based on search query
            var users = await _userService.SearchUsersByNameAsync(searchQuery);

            var model = new List<UserViewModel>();
            
            foreach (var user in users)
            {
                var friendship = await _friendshipService.CheckFriendship(user.Id, currentUser);
                model.Add(_mapper.MapToUserViewModel(user, friendship));
            }

            return View(model); // Return the view with results
        }
        [HttpPost]
        public async Task<IActionResult> RemoveProfilePicture(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if(user==null)
            {
                return NotFound();
            }
            if(user.Media!= null && user.Media.PublicId != null)
            {
                await _cloudinaryService.DeleteMediaAsync(user.Media.PublicId);
                await _mediaService.RemoveUserMediaAsync(userId);
            }

            return RedirectToAction("Profile", new { userId = userId });

        }
        [HttpGet]
        public async Task<IActionResult> Friends(string userId)
        {
            if(String.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var friends = await _friendshipService.GetAllFriends(userId);

            return View(_mapper.MapToFriendsViewModel(friends, userId));
        }
        [HttpGet]
        public async Task<IActionResult> SuggestedUsers(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var nonFriendsUsers = await _friendshipService.GetNonFriendUsers(userId);

            return View(_mapper.MapToUserViewModel(nonFriendsUsers.Item1,nonFriendsUsers.Item2));
        }

    }
}

