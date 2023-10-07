using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;

namespace Elastic.Feeder.Core.FileManagers
{
    public class ElasticFileManager : IElasticFileManager
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileManager(ILogger<ElasticFileObserver> logger) 
        { 
            _logger = logger;
        }

        public FileDetails ReadFile(string path)
        {
            _logger.LogInformation($"Reading file : {path}");
            try
            {
                var fileInfo = new FileInfo(path);
                var binaryData = GetBinaryFile(path);

                return new FileDetails
                {
                    FileName = fileInfo.Name,
                    Data = Convert.ToBase64String(binaryData)
                };
            }
            catch
            {
                _logger.LogError($"An error occured while reading file: {path}");
                throw;
            }
        }

        private byte[] GetBinaryFile(string filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            return bytes;
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
