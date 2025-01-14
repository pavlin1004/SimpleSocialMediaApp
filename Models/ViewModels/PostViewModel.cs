using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels
{
    public class PostViewModel
    { 

        public required Post Post { get; set; }
        public IEnumerable<Comment?> Comments { get; set; }
        public required bool HasReacted { get; set; }


    }
}
