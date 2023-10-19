using Microsoft.AspNetCore.Mvc;
using Redis.Sentinels.Services;
using StackExchange.Redis;

namespace Redis.Sentinels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet("[action]/{key}/{value}")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            IDatabase redis = await RedisSentinelService.RedisMasterDb();
            await redis.StringSetAsync(key, value);
            return Ok();
        }

        [HttpGet("[action]/{key}")]
        public async Task<IActionResult> GetValue(string key)
        {
            IDatabase redis = await RedisSentinelService.RedisMasterDb();
            RedisValue data = await redis.StringGetAsync(key);
            return Ok(data.ToString());
        }
    }
}
