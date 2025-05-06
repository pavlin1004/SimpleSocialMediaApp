using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;
using System.Security.Claims;

namespace SimpleSociaMedialApp.Services.Utilities.Implementations
{
    public class UserResolver: IUserResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SocialDbContext _context;
        public UserResolver(IHttpContextAccessor httpContextAccessor, SocialDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<AppUser?> ResolveCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _context.Users.FindAsync(userId);

            return user;
        }

        public string? ResolveCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
