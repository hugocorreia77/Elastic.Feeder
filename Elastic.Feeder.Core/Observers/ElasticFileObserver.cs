﻿using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Observers;
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
        private readonly ElasticFileReaderResolver _serviceResolver;


        public ElasticFileObserver(ILogger<ElasticFileObserver> logger, ElasticFileReaderResolver serviceResolver, 
            IOptions<ObserverConfiguration> configuration) 
        {
            _logger = logger;
            _serviceResolver = serviceResolver;
            _configuration = configuration.Value;
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


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _logger.LogInformation($"A new file appears! {e.FullPath}");

            var fileType = GetFileType(e.FullPath);
            var fileReaderService = _serviceResolver(fileType);

            fileReaderService.ReadFile(e.FullPath);
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