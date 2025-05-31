using System.Collections.Generic;
using System.Threading.Tasks;
using ShengWen.Server.Models;

namespace ShengWen.Server.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateTaskAsync(string audioUrl);
        Task<TaskItem?> GetNextTaskAsync();
        Task CompleteTaskAsync(string taskId, string transcript);
        Task<List<TaskItem>> GetTasksAsync();
    }
}
