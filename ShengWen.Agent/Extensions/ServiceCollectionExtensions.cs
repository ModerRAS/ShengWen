using Microsoft.Extensions.DependencyInjection;
using ShengWen.Agent.Services;

namespace ShengWen.Agent
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAgentServices(this IServiceCollection services)
        {
            services.AddSingleton<AgentClient>();
            return services;
        }
    }
}
