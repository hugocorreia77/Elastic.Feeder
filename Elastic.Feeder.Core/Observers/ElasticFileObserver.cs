using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Observers;
using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elastic.Feeder.Core.Observers
{
    public class ElasticFileObserver : IElasticFileObserver
    {
        private readonly ILogger<ElasticFileObserver> _logger;
        private readonly ObserverConfiguration _configuration;
        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();
        private readonly IElasticFileManager _serviceFileManager;
        private readonly IElasticFileService _documentService;

        public ElasticFileObserver(ILogger<ElasticFileObserver> logger, 
            IElasticFileManager serviceFileManager,
            IElasticFileService documentService,
            IOptions<ObserverConfiguration> configuration) 
        {
            _logger = logger;
            _serviceFileManager = serviceFileManager;
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

            try
            {
                var fileDetails = _serviceFileManager.ReadFile(e.FullPath);
                if(await _documentService.SaveFile(fileDetails))
                {
                    if(_configuration.DeleteLocalFileAfterUpload)
                    {
                        _serviceFileManager.DeleteFile(e.FullPath);
                    }
                    else
                    {
                        _logger.LogInformation("File {fileName} will not be removed from folder due to configurations.", fileDetails.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured. Please, check the reason.");
            }
        }

        private IEnumerable<string> GetFilters() 
            => _configuration.FileTypes.Split(',');

    }
}
