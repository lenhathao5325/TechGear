using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace TechGear.Service.CloudinaryService
{
    public class CloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        public CloudinaryService(Cloudinary acc) { _cloudinary = acc; }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "fruitstore")
        {
            if (file == null || file.Length == 0) return string.Empty;
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            var res = await _cloudinary.UploadAsync(uploadParams);
            if (res.StatusCode == System.Net.HttpStatusCode.OK) return res.SecureUrl.ToString();
            throw new Exception(res.Error?.Message ?? "Upload failed");
        }

        internal async Task UploadAsync(ImageUploadParams imageUploadParams)
        {
            throw new NotImplementedException();
        }
    }
}
