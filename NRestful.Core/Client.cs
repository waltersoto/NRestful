using NRestful.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NRestful.Core
{
    public class Client : IClient
    {
        private const char backSlash = '\\';
        private const char forwardSlash = '/';
        public Client(string urlAsString):this(!string.IsNullOrEmpty(urlAsString)? new Uri(urlAsString.TrimEnd(backSlash).TrimEnd(forwardSlash)) : null) { }
        public Client(Uri url) {
            if (url == null) return;
            ServiceUrl = url;
        }

        private static Uri UrlWithSegments(Uri uri, IDictionary<string, string> segments) {
            if (uri == null) return null;
            var url = uri.AbsoluteUri;
            if (segments == null) return uri;
          
            if (url.IndexOf('{',StringComparison.CurrentCultureIgnoreCase) == -1) return uri;
         
            var agreggated =  segments.Aggregate(url, (current, k) => current.Replace($"{{{k.Key.ToLower()}}}", k.Value));
            return new Uri(agreggated);
        }

        private static Uri UrlWithParameters(Uri uri, IDictionary<string, string> parameters) {
            var url = uri.AbsoluteUri;
            if (parameters == null) return uri;
            if (!parameters.Any()) return uri;

            var queryString = string.Join("&", parameters.Select(m => $"{m.Key}={m.Value}").ToList());

            return new Uri(url.IndexOf("?", StringComparison.Ordinal) == -1 ? $"{url}?{queryString}" : $"{url}&{queryString}");

        }

        public async Task<IResponse<TResponse>> GetAsync<TResponse>(Uri uri) {
            return await RequestAsync<TResponse, string>(new Request<string> {
                EndPoint = new EndPoint {
                    Method = Method.GET,
                    Uri = uri
                }
            }).ConfigureAwait(false);
        }

        public async Task<IResponse<TResponse>> DeleteAsync<TResponse>(Uri uri) {
            return await RequestAsync<TResponse, string>(new Request<string> {
                EndPoint = new EndPoint {
                    Method = Method.DELETE,
                    Uri = uri
                }
            }).ConfigureAwait(false);
        }

        public Uri ServiceUrl { get; set; }

        public Task<IResponse<TResponse>> PostAsync<TResponse>(Uri uri, string data) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = data,
                EndPoint = new EndPoint {
                    Method = Method.POST,
                    Uri = uri
                }
            });
        }

        public Task<IResponse<TResponse>> PutAsync<TResponse>(Uri uri, string data) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = data,
                EndPoint = new EndPoint {
                    Method = Method.PUT,
                    Uri = uri
                }
            });
        }

        public Task<IResponse<TResponse>> PatchAsync<TResponse>(Uri uri, string data) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = data,
                EndPoint = new EndPoint {
                    Method = Method.PATCH,
                    Uri = uri
                }
            });
        }
        public Task<IResponse<TResponse>> RequestAsync<TResponse>(IRequest request) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = request?.Data,
                EndPoint = request.EndPoint,
                Headers = request.Headers,
                Parameters = request.Parameters,
                UrlSegment = request.UrlSegment
            });
        }
        public async Task<IResponse<TResponse>> RequestAsync<TResponse, TRequestData>(IRequest<TRequestData> request) {

            var response = new Response<TResponse>();
            var isByteArray = typeof(TResponse) == typeof(byte[]);

            if (request?.EndPoint == null) return response;
            using (var client = new HttpClient()) {

                ByteArrayContent content = new StringContent(string.Empty);
                switch (typeof(TRequestData).FullName) {
                    case "System.String":
                        content = new StringContent(
                            request.Data as string ?? "",
                            Encoding.UTF8,
                            "application/json");
                        break;
                    case "System.Byte[]": {
                            var data = request.Data as byte[];
                            if (data != null) {
                                content = new ByteArrayContent(data, 0, data.Length);
                            }
                        }
                        break;
                    default: {
                            var data = request.Data.ObjectToSerializedByteArray();
                            content = data != null ? new ByteArrayContent(data, 0, data.Length) : new StringContent(string.Empty);
                        }
                        break;
                }

                var url = new Uri(FormatUrl(request.EndPoint.Uri, request.UrlSegment));

                HttpResponseMessage httpResponse;

                if (request.Headers != null && request.Headers.Count > 0) {
                    foreach (var header in request.Headers) {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var responseString = "";
                byte[] responseAsBytes = null;

                if (request.UrlSegment != null && request.UrlSegment.Any()) {
                    url = UrlWithSegments(url, request.UrlSegment);
                }
                if (request.Parameters != null && request.Parameters.Any()) {
                    url = UrlWithParameters(url, request.Parameters);
                }

                switch (request.EndPoint.Method) {
                    case Method.POST:
                        httpResponse = await client.PostAsync(url, content).ConfigureAwait(false);
                        break;
                    case Method.GET:
                        httpResponse = await client.GetAsync(url).ConfigureAwait(false);
                        break;
                    case Method.PUT:
                        httpResponse = await client.PutAsync(url, content).ConfigureAwait(false);
                        break;
                    case Method.DELETE:
                        httpResponse = await client.DeleteAsync(url).ConfigureAwait(false);
                        break;
                    case Method.PATCH:
                        httpResponse = await client.PatchAsync(url, content).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var statusCode = 520;
                var statusDescription = "Unknown Error";
                var successResult = false;

                if (httpResponse != null) {
                    if (isByteArray) {
                        responseAsBytes = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    }
                    else {
                        responseString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    statusCode = (int)httpResponse.StatusCode;
                    statusDescription = httpResponse.ReasonPhrase;
                    successResult = httpResponse.IsSuccessStatusCode;
                }

                if (isByteArray) {
                    if (responseAsBytes != null) {
                        response = new Response<TResponse>(responseAsBytes, successResult);
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(responseString)) {
                        response = new Response<TResponse>(responseString, successResult);
                    }
                }

                response.Status = statusCode;
                response.Description = statusDescription;

            }

            return response;
        }

        private string FormatUrl(Uri uri, IDictionary<string, string> segments) {
            var endPoint = uri.AbsoluteUri;
            endPoint = endPoint.TrimStart("/"[0])
                               .TrimStart(@"\"[0]);
            if (segments != null && segments.Count > 0) {
                endPoint = segments.Aggregate(endPoint,
                                    (current, val) =>
                                    current.Replace($"{{{val.Key}}}", val.Value,StringComparison.CurrentCulture));
            }

            return $"{ServiceUrl}/{endPoint}";
        }

    }
}
