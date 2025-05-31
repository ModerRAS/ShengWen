using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShengWen.Server.Models;

namespace ShengWen.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ConcurrentQueue<TaskItem> _taskQueue = new();
        private readonly ConcurrentDictionary<string, TaskItem> _tasks = new();

        public Task<TaskItem> CreateTaskAsync(string audioUrl)
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                AudioUrl = audioUrl,
                Status = Models.TaskStatus.Pending
            };

            _taskQueue.Enqueue(task);
            _tasks[task.Id] = task;
            
            return Task.FromResult(task);
        }

        public Task<TaskItem> GetNextTaskAsync()
        {
            if (_taskQueue.TryDequeue(out var task))
            {
                task.Status = Models.TaskStatus.Processing;
                return Task.FromResult(task);
            }
            return Task.FromResult<TaskItem>(null);
        }

        public Task CompleteTaskAsync(string taskId, string transcript)
        {
            if (_tasks.TryGetValue(taskId, out var task))
            {
                task.Transcript = transcript;
                task.Status = Models.TaskStatus.Completed;
            }
            return Task.CompletedTask;
        }

        public Task<List<TaskItem>> GetTasksAsync()
        {
            return Task.FromResult(_tasks.Values.ToList());
        }
    }
}
