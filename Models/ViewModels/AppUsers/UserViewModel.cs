using SimpleSocialApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Models.ViewModels.AppUsers
{
    public class UserViewModel
    {
        public required AppUser User { get; set; }
        public required bool IsFriend { get; set; }
    }
}
