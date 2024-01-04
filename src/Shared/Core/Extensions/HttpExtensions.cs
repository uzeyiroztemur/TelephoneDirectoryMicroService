using System.Text;

namespace Core.Extensions
{
    public enum HttpRequestMethod { Get, Post }

    public class HttpRequest
    {
        public HttpRequest()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }

        public string Url { get; set; }
        public HttpRequestMethod Method { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }

    public static class HttpExtensions
    {
        private static HttpRequestMessage GetRequestMessage(this HttpRequest httpRequest)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(httpRequest.Url),
                Method = new HttpMethod(Enum.GetName(typeof(HttpRequestMethod), httpRequest.Method)),
                Version = new Version(2, 0)
            };

            httpRequest.Headers.ForEach(header => httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value));

            if (httpRequest.Method == HttpRequestMethod.Post)
            {
                httpRequestMessage.Content = new StringContent(httpRequest.Body, Encoding.UTF8, httpRequest.ContentType);
            }

            return httpRequestMessage;
        }

        public static async Task<string> GetData(this HttpRequest httpRequest)
        {
            using var client = new HttpClient();
            using var res = await client.SendAsync(httpRequest.GetRequestMessage());
            using var content = res.Content;
            var data = await content.ReadAsStringAsync();
            return data;
        }
    }
}
