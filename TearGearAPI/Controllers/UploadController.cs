using Microsoft.AspNetCore.Mvc;
using TechGearAPI.Service.CloudinaryService;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public UploadController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// Upload image to Cloudinary
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="folder">Folder name in Cloudinary (default: techgear)</param>
        /// <returns>Image URL</returns>
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string folder = "techgear")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { message = "Invalid file type. Only images are allowed." });
                }

                // Validate file size (max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size must not exceed 5MB" });
                }

                var imageUrl = await _cloudinaryService.UploadImageAsync(file, folder);

                return Ok(new
                {
                    success = true,
                    message = "Image uploaded successfully",
                    imageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Upload failed",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Upload multiple images to Cloudinary
        /// </summary>
        /// <param name="files">List of image files</param>
        /// <param name="folder">Folder name in Cloudinary (default: techgear)</param>
        /// <returns>List of image URLs</returns>
        [HttpPost("images")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files, [FromQuery] string folder = "techgear")
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return BadRequest(new { message = "No files uploaded" });
                }

                var imageUrls = new List<string>();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                foreach (var file in files)
                {
                    // Validate each file
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        continue; // Skip invalid files
                    }

                    if (file.Length > 5 * 1024 * 1024)
                    {
                        continue; // Skip files larger than 5MB
                    }

                    var imageUrl = await _cloudinaryService.UploadImageAsync(file, folder);
                    imageUrls.Add(imageUrl);
                }

                if (imageUrls.Count == 0)
                {
                    return BadRequest(new { message = "No valid images were uploaded" });
                }

                return Ok(new
                {
                    success = true,
                    message = $"{imageUrls.Count} image(s) uploaded successfully",
                    imageUrls = imageUrls
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Upload failed",
                    error = ex.Message
                });
            }
        }
    }
}
