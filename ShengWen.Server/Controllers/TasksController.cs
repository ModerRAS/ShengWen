using Microsoft.AspNetCore.Mvc;
using ShengWen.Server.Services;
using ShengWen.Server.Models;

namespace ShengWen.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] string audioUrl)
        {
            var task = await _taskService.CreateTaskAsync(audioUrl);
            return Ok(task);
        }

        [HttpGet("next")]
        public async Task<IActionResult> GetNextTask()
        {
            var task = await _taskService.GetNextTaskAsync();
            if (task == null)
            {
                return NotFound("没有待处理任务");
            }
            Response.ContentType = "application/json";
            return Ok(task);
        }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteTask(
        [FromForm] string taskId, 
        [FromForm] string transcript)
    {
        await _taskService.CompleteTaskAsync(taskId, transcript);
        return Ok();
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _taskService.GetTasksAsync();
        return Ok(tasks);
    }
    }
}
