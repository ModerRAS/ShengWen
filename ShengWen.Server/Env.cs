using System.Data;

namespace ShengWen.Server {
    public class Env {
        public static readonly string JwtKey = Guid.NewGuid().ToString();
        public static readonly string WhisperApiKey = "your-whisper-api-key"; // 需要替换为实际API Key
        public static readonly string RedisConnection = "localhost:6379";
        public static readonly string AdminUsername = "admin";
        public static readonly string AdminPassword = "password";
    }
}
