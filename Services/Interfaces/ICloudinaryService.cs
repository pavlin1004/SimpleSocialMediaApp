namespace SimpleSocialApp.Services.Interfaces
{
    public interface ICloudinaryService
    {
        public Task<string> UploadMediaFileAsync(IFormFile file);    
    }
}
