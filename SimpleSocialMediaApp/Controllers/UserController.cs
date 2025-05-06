using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.AppUsers;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSociaMedialApp.Models.ViewModels.AppUsers;
using SimpleSociaMedialApp.Services.External.Interfaces;
using SimpleSociaMedialApp.Services.Functional.Interfaces;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;

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
        private readonly UserManager<AppUser> _userManager;


        public UserController(IUserService userService, IFriendshipService friendshipService, IPostService postService, IMediaService mediaService, ICloudinaryService cloudinaryService, IChatService chatService, ICommentService commentService,IMapper mapper, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _friendshipService = friendshipService;
            _postService = postService;
            _mediaService = mediaService;
            _cloudinaryService = cloudinaryService;
            _chatService = chatService;
            _commentService = commentService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userId, int size = 5, int count = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Fetch user data
            var user = await _userService.GetUserByIdAsync(userId); // Profile owner
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
            Friendship? friendshipStatus = await _friendshipService.CheckFriendship(currentUser.Id, userId);


            var viewModel = new ProfileViewModel
            {
                User = user,
                Posts = postViewModels,
                FriendshipStatus = friendshipStatus, // Null if no relationship exists
                IsCurrentUser = (user.Id == userId)
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
            var user = await _userManager.GetUserAsync(User);

            if (user.Id == userId) return BadRequest("You cannot send a friend request to yourself.");

            await _friendshipService.SendFriendshipRequestAsync(user.Id, userId); // Use "Pending" as status
            return RedirectToAction("Profile", new { userId });
        }
        [HttpPost]
        public async Task<IActionResult> AcceptFriendshipRequest(string userId)
        {
            var user = await _userManager.GetUserAsync(User);

            var friend = await _userService.GetUserByIdAsync(userId);
            if (user != null && friend != null)
            {
                await _chatService.CreatePrivateChatAsync(user, friend);
            } 
            await _friendshipService.AcceptUserFriendshipAsync(user.Id, userId);
            return RedirectToAction("Profile", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriendship(string userId)
        {
            var user = await _userManager.GetUserAsync(User);

            await _friendshipService.RemoveUserFriendshipsAsync(user.Id, userId);
            return RedirectToAction("Profile", new { userId });
        }

        // GET method to show the page to upload a profile picture
        [HttpGet]
        public async Task<IActionResult> EditProfilePicture()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentProfilePicture = user.Media;
           
            return View(currentProfilePicture); // Pass the profile picture to the view (if any)
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfilePicture(IFormFile mediaFile)
        {
            var user = await _userManager.GetUserAsync(User);

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
            return RedirectToAction("Profile", new { userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string searchQuery)
        {
            var currentUser = await _userManager.GetUserAsync(User);

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
                var friendship = await _friendshipService.CheckFriendship(currentUser.Id, user.Id);
                model.Add(_mapper.MapToUserViewModel(user, friendship));
            }

            return View(model); // Return the view with results
        }
        [HttpPost]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.Media!= null && user.Media.PublicId != null)
            {
                await _cloudinaryService.DeleteMediaAsync(user.Media.PublicId);
                await _mediaService.RemoveUserMediaAsync(user.Id);
            }

            return RedirectToAction("Profile", new { userId = user.Id });

        }
        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            var user = await _userManager.GetUserAsync(User);
            var friends = await _friendshipService.GetAllFriends(user.Id);

            return View(_mapper.MapToFriendsViewModel(friends, user.Id));
        }
        [HttpGet]
        public async Task<IActionResult> SuggestedUsers()
        {
            var user = await _userManager.GetUserAsync(User);
            var nonFriendsUsers = await _friendshipService.GetNonFriendUsers(user.Id);

            return View(_mapper.MapToUserViewModel(nonFriendsUsers.Item1,nonFriendsUsers.Item2));
        }

    }
}

