using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace SimpleSocialApp.Services
{
    public class CloudinaryService: ICloudinaryService
    {

        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

  
        public async Task<string> UploadImageAsync(IFormFile image)
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
                    return uploadResult.SecureUrl.ToString(); 
                }
            }

            return String.Empty;
        }

        public async Task<string> UploadVideoAsync(IFormFile video)
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
                    return uploadResult.SecureUrl.ToString(); 
                }
            }

            return String.Empty;
        }
    }
}
