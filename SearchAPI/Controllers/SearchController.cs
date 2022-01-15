using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SearchAPI.Search;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
        public IActionResult GetResults(string search_term)
        {
            IEnumerable<KeyValuePair<int, JToken>> result = _searchProvider.SearchRecords(search_term);
            var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false } };

            var obj1 = JsonConvert.SerializeObject(result,settings);
            

            return Ok(obj1);
        }
    }
}
