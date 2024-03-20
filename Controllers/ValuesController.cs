using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("Test")]
        [HttpPost]
        public string Post([FromBody] object data)
        {
            dynamic a = JsonConvert.DeserializeObject(data.ToString());

            return $"id: {a.id}; name: {a.name}";
        }
    }
}
