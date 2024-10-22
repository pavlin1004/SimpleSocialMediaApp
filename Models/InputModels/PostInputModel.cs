namespace SimpleSocialApp.Models.InputModels
{
    public class PostInputModel
    {
        public string? Data { get; set; }

        public ICollection<IFormFile>? Media;

    }
}
