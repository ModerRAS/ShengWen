using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace ShengWen.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AgentsController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public AgentsController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var db = _redis.GetDatabase();
            var keys = await db.ExecuteAsync("KEYS", "agent:*");

            var agents = new List<AgentStatus>();
            foreach (var key in (RedisResult[])keys)
            {
                var value = await db.StringGetAsync((string)key);
                var agent = JsonSerializer.Deserialize<AgentStatus>(value);
                agent.Status = GetAgentStatus(agent.LastActiveTime);
                agents.Add(agent);
            }

            return Ok(agents);
        }

        private string GetAgentStatus(DateTime lastActiveTime)
        {
            var minutesSinceLastActive = (DateTime.UtcNow - lastActiveTime).TotalMinutes;
            return minutesSinceLastActive switch
            {
                < 1 => "在线",
                < 5 => "忙碌",
                _ => "离线"
            };
        }

        public class AgentStatus
        {
            public string Id { get; set; }
            public string IpAddress { get; set; }
            public DateTime LastActiveTime { get; set; }
            public double CpuUsage { get; set; }
            public double MemoryUsage { get; set; }
            public string Status { get; set; }
        }
    }
}
