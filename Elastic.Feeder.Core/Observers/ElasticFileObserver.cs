using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Observers;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Core.Readers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elastic.Feeder.Core.Observers
{
    public class ElasticFileObserver : IElasticFileObserver
    {
        private readonly ILogger<ElasticFileObserver> _logger;
        private readonly ObserverConfiguration _configuration;
        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();
        private readonly ElasticFileReaderResolver _serviceReaderResolver;
        private readonly IDocumentService _documentService;

        public ElasticFileObserver(ILogger<ElasticFileObserver> logger, ElasticFileReaderResolver serviceReaderResolver,
            IDocumentService documentService,
            IOptions<ObserverConfiguration> configuration) 
        {
            _logger = logger;
            _serviceReaderResolver = serviceReaderResolver;
            _configuration = configuration.Value;
            _documentService = documentService;
        }

        public Task Observe()
        {
            _logger.LogInformation($"Starting to observe {_configuration.Folder}");

            if (!Directory.Exists(_configuration.Folder))
            {
                var errorMessage = $"Directory doesn't exists! Directory: {_configuration.Folder}";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            foreach(var filter in GetFilters())
            {
                var watcher = new FileSystemWatcher(_configuration.Folder);
                watcher.NotifyFilter = NotifyFilters.Attributes |
                                            NotifyFilters.CreationTime |
                                            NotifyFilters.DirectoryName |
                                            NotifyFilters.FileName |
                                            NotifyFilters.LastAccess |
                                            NotifyFilters.LastWrite |
                                            NotifyFilters.Security |
                                            NotifyFilters.Size; ;
                watcher.Filter = filter;
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.EnableRaisingEvents = true;

                _watchers.Add(watcher);
            }

            return Task.CompletedTask;
        }


        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            _logger.LogInformation($"A new file appears! {e.FullPath}");

            var fileType = GetFileType(e.FullPath);
            var fileReaderService = _serviceReaderResolver(fileType);

            try
            {
                var jsonData = await fileReaderService.ReadFileAsync(e.FullPath);

                var encodedData = Base64Encode(jsonData);

                await _documentService.SaveDocument(encodedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured. Please, check the reason.");
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private string GetFileType(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Extension.Substring(1);
        }

        private IEnumerable<string> GetFilters() 
            => _configuration.FileTypes.Split(',');

    }
}
