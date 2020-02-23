
using System.Net.Http;
using System.Threading.Tasks;

namespace NRestful.Core {
    public static class PatchAsyncExtension {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content) {
            HttpResponseMessage response = null;
            
            if(client != null) {
                using(var message = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) {
                    Content = content
                }) {
                    response = await client.SendAsync(message).ConfigureAwait(false);
                }
            }
 
            return response;
        }

    }
}
