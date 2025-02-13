namespace SimpleSocialApp.Services.Interfaces
{
    public interface ICloudinaryService
    {
        public Task<(string,string)> UploadMediaFileAsync(IFormFile file);
        public Task<bool> DeleteMediaAsync(string publicId);

    }
}
