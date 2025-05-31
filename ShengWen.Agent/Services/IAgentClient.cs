using System.Net;
using System.Threading.Tasks;

namespace ShengWen.Agent.Services
{
    public interface IAgentClient
    {
        Task ConnectAsync(IPEndPoint endpoint);
    }
}
