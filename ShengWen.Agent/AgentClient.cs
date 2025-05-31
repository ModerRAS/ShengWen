using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ShengWen.Agent
{
    public class AgentClient : IDisposable
    {
        private TcpClient _client;
        private bool _disposed;

        public bool IsConnected => _client?.Connected ?? false;

        public async Task ConnectAsync(IPEndPoint endpoint)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(endpoint.Address, endpoint.Port);
        }

        public async Task DisconnectAsync()
        {
            if (_client != null && _client.Connected)
            {
                await Task.Run(() => _client.Close());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _client?.Dispose();
            }

            _disposed = true;
        }
    }
}
