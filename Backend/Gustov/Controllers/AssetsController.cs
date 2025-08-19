using Microsoft.AspNetCore.Mvc;

namespace Gustov.Controllers
{
    [ApiController]
    [Route("assets")]
    public class AssetsController : ControllerBase
    {
        [HttpGet("images/{imageName}")]
        public IActionResult GetUserAvatar(string imageName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), $"assets/images/{imageName}");

            if (!System.IO.File.Exists(filepath))
            {
                return NotFound(new { message = "File not found" });
            }

            var fileExtension = Path.GetExtension(imageName).ToLower();

            string contentType = fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"  // Default for unsupported file types
            };

            var filestream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            return File(filestream, contentType);
        }
    }
}
