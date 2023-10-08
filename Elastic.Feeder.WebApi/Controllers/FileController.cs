using Elastic.Feeder.Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.Feeder.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IElasticFileService _documentService;


        public FileController(ILogger<FileController> logger, IElasticFileService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        
        
        [HttpGet("search")]
        public async Task<IEnumerable<string>> Search(string search)
        {
            var searchResults = await _documentService.SearchFileContentAsync(search);
            return searchResults;
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var searchResult = await _documentService.GetFileDataAsync(fileName);

                var binaryData = Convert.FromBase64String(searchResult);
                var content = new MemoryStream(binaryData);
                var contentType = "application/octet-stream";

                return File(content, contentType, fileName);
            }
            catch(ArgumentException ae)
            {
                return NotFound(ae.Message);
            }
        }
    }
}