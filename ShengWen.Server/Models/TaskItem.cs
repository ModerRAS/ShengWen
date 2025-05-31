namespace ShengWen.Server.Models
{
    public class TaskItem
    {
        public string Id { get; set; } = "";
        public string AudioUrl { get; set; } = "";
        public string Transcript { get; set; } = "";
        public TaskStatus Status { get; set; }
    }
}