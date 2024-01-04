using Newtonsoft.Json;

namespace Core.Entities.DTOs
{
    public class DataResult<T> : Result, IDataResult<T>, IResult
    {
        public DataResult(T data, int count, bool success, string message)
          : base(success, message)
        {
            this.Data = data;
            this.Count = count;
        }

        public DataResult(T data, bool success, string message)
          : base(success, message)
        {
            this.Data = data;
        }

        public DataResult(T data, int count, bool success)
          : base(success)
        {
            this.Data = data;
            this.Count = count;
        }

        public DataResult(T data, bool success)
          : base(success)
        {
            this.Data = data;
        }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
