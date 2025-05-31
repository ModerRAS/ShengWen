using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Net.Http;
using System.Threading.Tasks;
using ShengWen.Server.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ShengWen.Server.IntegrationTests
{
    public class ServerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ServerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Development");
                });
        }

        [Fact]
        public async Task GetTasks_ReturnsSuccessAndJsonContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Tasks");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", 
                response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task GetNextTask_ReturnsValidResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Tasks/next");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json",
                response.Content.Headers.ContentType?.MediaType);
        }
    }
}
