using Newtonsoft.Json;

namespace Core.Entities.DTOs
{
    public class Result : IResult
    {
        public Result()
        {
            
        }

        public Result(bool success, string message)
          : this(success)
        {
            this.Message = message;
        }

        public Result(bool success) => this.Success = success;

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
