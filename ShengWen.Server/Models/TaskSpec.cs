using ShengWen.Server.Enums;

namespace ShengWen.Server.Models {
    public class TaskSpec {
        public Guid UUID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public TaskType TaskType { get; set; }
    }
}
