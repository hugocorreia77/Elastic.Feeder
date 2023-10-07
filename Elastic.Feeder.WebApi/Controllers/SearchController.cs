using Elastic.Feeder.Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.Feeder.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IElasticFileService _documentService;


        public SearchController(ILogger<SearchController> logger, IElasticFileService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet(Name = "")]
        public async Task<IEnumerable<string>> SearchAsync(string search)
        {
            var searchResults = await _documentService.SearchFileContentAsync(search);
            return searchResults;
        }
    }
}