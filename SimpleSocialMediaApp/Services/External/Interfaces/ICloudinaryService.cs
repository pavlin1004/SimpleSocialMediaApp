namespace SimpleSociaMedialApp.Services.External.Interfaces
{
    public interface ICloudinaryService
    {
        public Task<List<string>?> UploadMediaFileAsync(IFormFile file);
        public Task<bool> DeleteMediaAsync(string publicId);

    }
}
