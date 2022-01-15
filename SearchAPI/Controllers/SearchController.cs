using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SearchAPI.Search;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly SearchProvider _searchProvider;
        public SearchController()
        {
            this._searchProvider = new SearchProvider();
        }
        [HttpGet("{search_term}")]
        public async Task<IActionResult> GetResults(string search_term)
        {
            SortedDictionary<int, JToken> keyValuePairs = _searchProvider.SearchRecords(search_term);

            return Ok(keyValuePairs);
        }
    }
}
