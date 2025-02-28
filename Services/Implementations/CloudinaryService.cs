using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http.Metadata;
using NuGet.Protocol;
using SimpleSocialApp.Services.Interfaces;


namespace SimpleSocialApp.Services.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {

        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<(string,string)> UploadMediaFileAsync(IFormFile file)
        {
            if (file.ContentType.StartsWith("image/"))
            {
                return await UploadImageAsync(file);
            }
            if(file.ContentType.StartsWith("video/"))
            {
                return await UploadVideoAsync(file);
            }
         
            return (String.Empty,String.Empty);
        }





        private async Task<(string,string)> UploadImageAsync(IFormFile image)
        {
            if (image.Length > 0)
            {
                using (var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, stream),
                        Transformation = new Transformation()
                            .Width(500)
                            .Height(500)
                            .Crop("fill")
                            .Gravity("face")
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return (uploadResult.SecureUrl.ToString(),uploadResult.PublicId);
                }
            }

            return (String.Empty, String.Empty);
        }
        private async Task<(string,string)> UploadVideoAsync(IFormFile video)
        {
            if (video.Length > 0)
            {
                using (var stream = video.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(video.FileName, stream)
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return (uploadResult.SecureUrl.ToString(),uploadResult.PublicId);
                }
            }
            return (String.Empty,String.Empty);
        }

        public async Task<bool> DeleteMediaAsync(string publicId)
        {
            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);
                return result.Result == "ok";
            }
            return false;
        }
    }
}
