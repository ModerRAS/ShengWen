using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShengWen.Agent.Services
{
public class AgentClient : IAgentClient
    {
        private readonly HttpClient _httpClient;

        public AgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ConnectAsync(IPEndPoint endpoint)
        {
            // Implementation of connection logic
            await Task.CompletedTask;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
