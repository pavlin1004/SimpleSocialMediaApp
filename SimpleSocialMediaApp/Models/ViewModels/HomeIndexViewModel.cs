using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.Posts;

namespace SimpleSocialApp.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public required ICollection<PostViewModel> Posts { get; set; }
    }
}
