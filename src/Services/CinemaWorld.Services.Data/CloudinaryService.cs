namespace CinemaWorld.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    using CinemaWorld.Services.Data.Contracts;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            byte[] destinationImage;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            destinationImage = memoryStream.ToArray();

            using var destinationStream = new MemoryStream(destinationImage);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, destinationStream),
            };

            var result = await this.cloudinary.UploadAsync(uploadParams);

            return result.Uri.AbsoluteUri;
        }
    }
}
