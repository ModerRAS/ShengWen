using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShengWen.Server.Models;
using StackExchange.Redis;

namespace ShengWen.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AddTaskController : ControllerBase {
        public IConnectionMultiplexer connectionMultiplexer { get; set; }
        public AddTaskController(IConnectionMultiplexer connectionMultiplexer) {
            this.connectionMultiplexer = connectionMultiplexer;
        }
        [HttpPost(Name = "AddTask")]
        [Authorize]
        public async Task<string> Post([FromForm] AddTaskDTO task) {
            if (!connectionMultiplexer.IsConnected) {
                return string.Empty;
            }
            var db = connectionMultiplexer.GetDatabase();
            var uuid = Guid.NewGuid();
            var taskSpec = new TaskSpec() { FileName = task.FileName, TaskType = task.TaskType, UUID = uuid, FileUrl = task.FileUrl };

            await db.ListRightPushAsync("TaskList", JsonConvert.SerializeObject(taskSpec));
            return uuid.ToString();
        }
    }
}
