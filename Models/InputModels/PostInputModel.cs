using SimpleSocialApp.Models.Validation;

namespace SimpleSocialApp.Models.InputModels
{

    public class PostInputModel
    {
        [EnsureOneRequired("Data", "Media")]
        public string? Data { get; set; }

        public ICollection<IFormFile>? Media;

    }
}
