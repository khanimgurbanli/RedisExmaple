using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;

namespace InMemory.Caching.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public WeatherForecastController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("set")]
        public async Task Set(string value)
        {
            _memoryCache.Set("variable", value);
        }

        [HttpGet("get")]
        public string Get()
        {
            if (_memoryCache.TryGetValue<string>("variable", out string value))
            {
                value = _memoryCache.Get<string>("variable");
                return value;
            }

            return string.Empty;
        }

        //Absolute - Sliding Expriation
        [HttpGet("setData")]
        public void SetDate(string value)
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet("getData")]
        public DateTime GetDate(string value)
        {
            return _memoryCache.Get<DateTime>("date");
        }
    }
}