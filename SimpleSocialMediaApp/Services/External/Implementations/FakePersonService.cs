using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using SimpleSociaMedialApp.Services.External.Interfaces;

namespace SimpleSociaMedialApp.Services.External.Implementations
{
    public class FakePersonService : IFakePersonService
    {
        private readonly HttpClient _httpClient;

        public FakePersonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> FetchRandomImageAsync()
        {
            var url = "https://thispersondoesnotexist.com/";
            var response = await _httpClient.GetByteArrayAsync(url);

            // Get the wwwroot folder path
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Target directory inside wwwroot
            string relativePath = "images/profile";

            // Absolute path to save the file
            string absolutePath = Path.Combine(wwwRootPath, relativePath);

            // Ensure the directory exists
            Directory.CreateDirectory(absolutePath);

            // Generate a unique filename
            string fileName = $"{Guid.NewGuid()}.jpeg";
            string absoluteFilePath = Path.Combine(absolutePath, fileName);

            // Save image locally
            await File.WriteAllBytesAsync(absoluteFilePath, response);

            Console.WriteLine($"Image saved successfully to: {absoluteFilePath}");

            // Return the relative URL for browser access
            return $"/{relativePath}/{fileName}";
        }

        public string DetectGenderAsync(string imagePath)
        {
            // Convert the relative path to an absolute path
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string absolutePath = Path.Combine(wwwRootPath, imagePath.TrimStart('/'));

            var image = new Image<Bgr, byte>(absolutePath);

            // Haar Cascade file path
            string cascadePath = @"C:\haar\haarcascade_frontalface_default.xml";
            var faceCascade = new CascadeClassifier(cascadePath);

            // Detect faces
            var faces = faceCascade.DetectMultiScale(image, 1.1, 10);

            string genderResult = "Unknown";

            foreach (var face in faces)
            {
                var faceRegion = image.Copy(face).Convert<Gray, byte>();

                // Simple brightness-based gender detection
                var brightness = faceRegion.GetAverage().Intensity;

                genderResult = brightness > 100 ? "Female" : "Male";
            }

            return genderResult;
        }
    }
}
