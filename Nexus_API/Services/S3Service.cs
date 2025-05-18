using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Nexus_API.Services
{
    public interface IS3Service
    {
        Task<Stream> GetDocumentAsync(string bucketName, string objectKey);
        Task<string> UploadDocumentAsync(string bucketName, string objectKey, Stream fileStream);
        Task<bool> DeleteDocumentAsync(string bucketName, string objectKey);
    }

    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        public S3Service(IConfiguration configuration)
        {
            _configuration = configuration;

            var config = new AmazonS3Config
            {
                ServiceURL = _configuration["YandexS3:ServiceURL"],
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(
                _configuration["YandexS3:AccessKey"],
                _configuration["YandexS3:SecretKey"],
                config
            );
        }

        public async Task<Stream> GetDocumentAsync(string bucketName, string objectKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                using var response = await _s3Client.GetObjectAsync(request);
                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return memoryStream;
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error getting object {objectKey} from bucket {bucketName}: {ex.Message}");
            }
        }

        public async Task<string> UploadDocumentAsync(string bucketName, string objectKey, Stream fileStream)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = fileStream
                };

                await _s3Client.PutObjectAsync(request);
                return $"https://{bucketName}.storage.yandexcloud.net/{objectKey}";
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error uploading object {objectKey} to bucket {bucketName}: {ex.Message}");
            }
        }

        public async Task<bool> DeleteDocumentAsync(string bucketName, string objectKey)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                await _s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error deleting object {objectKey} from bucket {bucketName}: {ex.Message}");
            }
        }
    }
}