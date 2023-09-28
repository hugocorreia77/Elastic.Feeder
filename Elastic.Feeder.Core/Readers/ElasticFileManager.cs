using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;

namespace Elastic.Feeder.Core.Readers
{
    public class ElasticFileManager : IElasticFileManager
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileManager(ILogger<ElasticFileObserver> logger) 
        { 
            _logger = logger;
        }

        public async Task<FileDetails> ReadFileAsync(string path)
        {
            _logger.LogInformation($"Reading file : {path}");
            try
            {
                using StreamReader reader = new(path);
                var fileData = await reader.ReadToEndAsync();

                _logger.LogInformation("File Content:");

                var fileInfo = new FileInfo(path);

                return new FileDetails
                {
                    FileName = fileInfo.Name,
                    Data = Base64Encode(fileData)
                };
            }
            catch
            {
                _logger.LogError($"An error occured while reading file: {path}");
                throw;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.LogInformation("File {filename} deleted successfully!", path);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "It was not possivle to delete de file {filename}", path);
            }
            return false;
        }
    }
}
