using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace ShengWen.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApiKeysController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, ApiKey> _apiKeys = new();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_apiKeys.Values.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateApiKeyRequest request)
        {
            var key = new ApiKey
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Value = GenerateApiKey(),
                CreatedAt = DateTime.UtcNow
            };

            _apiKeys.TryAdd(key.Id, key);
            return Ok(key);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _apiKeys.TryRemove(id, out _);
            return NoContent();
        }

        private string GenerateApiKey()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "");
        }

        public class CreateApiKeyRequest
        {
            public string Name { get; set; }
        }

        public class ApiKey
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
