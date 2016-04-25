
using System.Net.Http;
using System.Threading.Tasks;

namespace NRestful {
    public static class PatchAsyncExtension {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content) {
            return await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) {
                Content = content
            });
        }

    }
}
