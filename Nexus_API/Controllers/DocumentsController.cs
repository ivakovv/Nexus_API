using Microsoft.AspNetCore.Mvc;
using Nexus_API.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Nexus_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IS3Service s3Service, ILogger<DocumentsController> logger)
        {
            _s3Service = s3Service;
        }

        [HttpGet("client-photo/{clientId}")]
        public async Task<IActionResult> GetClientPhoto(string clientId)
        {
            try
            {

                var objectKey = $"clients/{clientId}";
                var bucketName = "nexus-bank"; 

                var fileStream = await _s3Service.GetDocumentAsync(bucketName, objectKey);

                return File(fileStream, "image/jpeg", $"{clientId}");
            }
            catch (Exception ex)
            {
                return NotFound($"Client photo not found for client {clientId}");
            }
        }

        [HttpPost("client-photo/{clientId}")]
        public async Task<IActionResult> UploadClientPhoto(string clientId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var objectKey = $"clients/{clientId}";
                var bucketName = "nexus-bank"; 

                using var stream = file.OpenReadStream();
                var fileUrl = await _s3Service.UploadDocumentAsync(bucketName, objectKey, stream);

                return Ok(new { PhotoUrl = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error uploading client photo");
            }
        }

        [HttpDelete("client-photo/{clientId}")]
        public async Task<IActionResult> DeleteClientPhoto(string clientId)
        {
            try
            {
                var objectKey = $"clients/{clientId}";
                var bucketName = "nexus-bank"; 

                await _s3Service.DeleteDocumentAsync(bucketName, objectKey);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting client photo");
            }
        }
    }
}