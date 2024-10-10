namespace SimpleSocialApp.Services
{
    public interface ICloudinaryService
    {
        public Task<string> UploadImageAsync(IFormFile image); // returns Url
        public Task<string> UploadVideoAsync(IFormFile video); // returns Url
    }
}
