using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels;
using SimpleSocialApp.Services.Interfaces;
using System.Drawing;
using System.Security.Claims;

namespace SimpleSocialApp.Controllers
{

    public class FriendshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService) 
        {
            _friendshipService= friendshipService;
        }

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(String.IsNullOrEmpty(userId))
            {
                return BadRequest("Could not retrueve user ID");
            }
            var requests = await _friendshipService.GetUserPendingFriendshipsAsync(userId);


            var viewModel = requests.Select(request => new FriendshipRequestViewModel
            {
                FriendshipId = request.Id,
                UserFromName = request.Sender.Id,
                DateSent = request.CreatedAt
            }).ToList();

            return View(viewModel);         
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(FriendshipRequestViewModel m)
        {
            var result = await _friendshipService.AcceptUserFriendshipAsync(m.FriendshipId);
            if(!result)
            {
                return NotFound();
            }
            return RedirectToAction("Request");
        }


        [HttpPost]
        public async Task<IActionResult> RejectRequest(FriendshipRequestViewModel m)
        {
            var result = await _friendshipService.RejectUserFriendshipsAsync(m.FriendshipId);
            if(!result)
            {
                return NotFound();
            }
            return RedirectToAction("Request");
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(string userToId)
        {        

            var userFromId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userFromId != null)
            {
                var friendship = new Friendship
                {
                    SenderId = userFromId,
                    ReceiverId = userToId,
                    CreatedAt = DateTime.UtcNow,
                    Type = FriendshipType.Pending
                };
                
                await _friendshipService.CreateFriendshipRequestAsync(friendship);

                return RedirectToAction("Requests");
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
