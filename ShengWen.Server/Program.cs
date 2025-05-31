
using Microsoft.Extensions.FileProviders; // Add for PhysicalFileProvider
using Garnet;
using System.Net.Sockets;
using System.Net;
using StackExchange.Redis;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tsavorite.core;
using ShengWen.Server.Services;
using ShengWen.Server.Models;

namespace ShengWen.Server {
    public class Program {
        public static int GetRandomAvailablePort() {
            // 尝试使用一个随机端口
            TcpListener listener = null;
            try {
                // 创建一个监听器，绑定到任意端口
                listener = new TcpListener(IPAddress.Loopback, 0); // 0表示随机端口
                listener.Start();

                // 获取分配的端口号
                int port = ((IPEndPoint)listener.LocalEndpoint).Port;

                return port;
            } catch (Exception) {
                // 若端口绑定失败，则返回-1
                return -1;
            } finally {
                // 关闭监听器
                listener?.Stop();
            }
        }
        public static void Main(string[] args) {
            var port = GetRandomAvailablePort();
            try {
                var server = new GarnetServer(["bind", "127.0.0.1", "--port", $"{port}"]);
                server.Start();
            } catch (Exception ex) {
                Console.WriteLine($"Unable to initialize server due to exception: {ex.Message}");
            }
            var builder = WebApplication.CreateBuilder(args);
            // 设置服务器端口为5000
            builder.WebHost.UseUrls("http://localhost:5000");

            //// 配置 JWT 认证
            //var jwtKey = "your-secret-key"; // 替换为你的密钥
            var key = Encoding.UTF8.GetBytes(Env.JwtKey);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true, // 是否验证 Issuer
                        ValidateAudience = true, // 是否验证 Audience
                        ValidateLifetime = true, // 是否验证 Token 过期时间
                        ValidateIssuerSigningKey = true, // 是否验证密钥
                        ValidIssuer = "your-issuer", // 你的 Issuer
                        ValidAudience = "your-audience", // 你的 Audience
                        IssuerSigningKey = new SymmetricSecurityKey(key) // 你的密钥
                    };
                });

            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"localhost:{port}"));
            builder.Services.AddSingleton<ITaskService, TaskService>(); // 注册任务服务


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Configure static files middleware
            var clientAppPath = Path.Combine(builder.Environment.ContentRootPath, "../ShengWen.Client/dist");
            if (!Directory.Exists(clientAppPath))
            {
                Directory.CreateDirectory(clientAppPath);
            }

            // 配置静态文件和路由
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(clientAppPath)
            });
            app.UseRouting();

            // 配置端点
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers().RequireAuthorization();
                
                // SPA回退路由
                endpoints.MapFallbackToFile("index.html", new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(clientAppPath)
                });
            });

            app.Run();
        }
    }
}
