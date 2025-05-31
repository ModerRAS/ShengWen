using ShengWen.Server.Enums;

namespace ShengWen.Server.Models {
    public class AddTaskDTO {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public TaskType TaskType { get; set; }
    }
}
