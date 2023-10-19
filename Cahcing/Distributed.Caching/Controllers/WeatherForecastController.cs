using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public WeatherForecastController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> Set(string value)
        {
            await _distributedCache.SetStringAsync("data", value, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });

            //set data as a binary format
            await _distributedCache.SetAsync("data", Encoding.UTF8.GetBytes(value), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });
            return Ok();
        }


        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            string valueString = await _distributedCache.GetStringAsync("data");

            var valueBinary = await _distributedCache.GetAsync("data");

            string valueCahngeString = Encoding.UTF8.GetString(valueBinary);

            return Ok(new
            {
                valueString,
                valueBinary,
                valueCahngeString
            });
        }
    }
}