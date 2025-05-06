using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Services.Utilities.Interfaces
{
    public interface IUserResolver 
    {
        public Task<AppUser?> ResolveCurrentUserAsync();
        public string? ResolveCurrentUserId();

    }
}
