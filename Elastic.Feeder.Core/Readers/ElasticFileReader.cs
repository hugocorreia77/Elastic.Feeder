using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;

namespace Elastic.Feeder.Core.Readers
{
    public abstract class ElasticFileReader : IElasticFileReader
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileReader(ILogger<ElasticFileObserver> logger) 
        { 
            _logger = logger;
        }

        public abstract Task ReadFile(string path);

        public bool IsFileLocked(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }



    }
}
