using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public required IEnumerable<Post> Posts { get; set; }
    }
}
