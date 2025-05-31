using System;
using System.Threading.Tasks;
using Xunit;
using ShengWen.Agent.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ShengWen.Agent.IntegrationTests
{
    public class AgentIntegrationTests
    {
        [Fact]
        public async Task Agent_CanConnectToServer()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHttpClient<AgentClient>(client => 
                    {
                        client.BaseAddress = new Uri("http://localhost:5000");
                    });
                })
                .Build();

            var agent = host.Services.GetRequiredService<AgentClient>();
            
            // Test connection logic
            var endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 5000);
            await agent.ConnectAsync(endpoint);
            // If no exception thrown, connection was successful
            Assert.True(true);
        }
    }
}
