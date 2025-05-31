using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Whisper.net.Ggml;
using ShengWen.Agent.Services;
using Microsoft.Extensions.Configuration;

namespace ShengWen.Agent {
    public class Program {
        private const string ModelName = "ggml-large-v3-q5_0.bin";
        
        static async Task Main() {
            // 加载配置
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 初始化Whisper模型
            if (!File.Exists(ModelName)) {
                Console.WriteLine("正在下载Whisper模型...");
                using var httpClient = new HttpClient();
                var modelUrl = config["ModelUrl"] ?? "https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-large-v3-q5_0.bin";
                var response = await httpClient.GetAsync(modelUrl);
                await using var modelStream = await response.Content.ReadAsStreamAsync();
                await using var fileWriter = File.OpenWrite(ModelName);
                await modelStream.CopyToAsync(fileWriter);
            }

            // 初始化音频处理服务
            var audioService = new Services.AudioProcessingService();
            
            // 从配置获取服务器URL和Token
            string apiBaseUrl = config["ServerUrl"] ?? "http://localhost:5000";
            string? token = config["Token"];
            
            if (string.IsNullOrEmpty(token)) {
                Console.WriteLine("❌ 配置中缺少Token，程序退出");
                return;
            }
            Console.WriteLine($"🔑 使用配置Token: {token.Substring(0, Math.Min(20, token.Length))}...");
            
            // 任务处理循环
            while (true) {
                Console.WriteLine("⌛ 正在查询待处理任务...");
                var task = await FetchNextTaskAsync(apiBaseUrl, token);
                
                if (task == null) {
                    Console.WriteLine("⏳ 无待处理任务，10秒后重试");
                    await Task.Delay(10000);
                    continue;
                }
                
                try {
                    Console.WriteLine($"✅ 获取到任务 {task.Id}");
                    Console.WriteLine($"⬇️ 下载音频文件: {task.AudioUrl}");
                    
                    // 下载音频文件
                    var audioData = await DownloadAudioAsync(task.AudioUrl);
                    if (audioData == null || audioData.Length == 0) {
                        Console.WriteLine("❌ 音频下载失败");
                        continue;
                    }
                    
                    // 生成SRT字幕
                    var srtContent = await audioService.GenerateSrtAsync(audioData);
                    Console.WriteLine("🎬 SRT生成完成");
                    
                    // 推送结果到服务端
                    await PushResultToServerAsync(apiBaseUrl, token, srtContent, task.Id);
                    
                    Console.WriteLine("✅ 任务处理完成");
                } catch (Exception ex) {
                    Console.WriteLine($"❌ 任务处理失败: {ex.Message}");
                }
                
                // 间隔5秒处理下一个任务
                await Task.Delay(5000);
            }

        }

        /// <summary>
        /// 调用受保护的 API 端点
        /// </summary>
        static async Task PushResultToServerAsync(string apiBaseUrl, string token, string srtContent, string taskId) {
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                // 创建推送内容
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(taskId), "TaskId");
                content.Add(new StringContent(srtContent), "Transcript");
                content.Add(new StringContent("1"), "ResultType"); // 1表示SRT类型
                
                HttpResponseMessage response = await client.PostAsync($"{apiBaseUrl}/api/Tasks/complete", content);

                if (response.IsSuccessStatusCode) {
                    string responseText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"✅ 结果推送成功: {responseText}");
                } else {
                    Console.WriteLine($"❌ 结果推送失败: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
        }

        static async Task<TaskItem?> FetchNextTaskAsync(string apiBaseUrl, string token) {
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync($"{apiBaseUrl}/api/Tasks/next");
                
                if (response.IsSuccessStatusCode) {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TaskItem>(responseJson);
                }
                
                Console.WriteLine($"❌ 获取任务失败: {response.StatusCode}");
                return null;
            }
        }
        
        static async Task<byte[]?> DownloadAudioAsync(string audioUrl) {
            using (HttpClient client = new HttpClient()) {
                try {
                    return await client.GetByteArrayAsync(audioUrl);
                } catch {
                    return null;
                }
            }
        }
        
        class TaskItem {
            public string Id { get; set; } = "";
            public string AudioUrl { get; set; } = "";
        }
    }
}
