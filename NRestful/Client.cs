﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NRestful.Interfaces;

namespace NRestful {
    public class Client : IClient {

        public Client(string url) {

            if (url == null) return;

            ServiceUrl = url.TrimEnd(@"\"[0])
                            .TrimEnd("/"[0]);
        }

        private static string UrlWithSegments(string url, IDictionary<string, string> segments) {

            if (segments == null) return url;
            if (string.IsNullOrEmpty(url)) return "";

            if (url.IndexOf('{') == -1) return url;
            url = url.ToLower();

            return segments.Aggregate(url,
                  (current, k) => current.Replace($"{{{k.Key.ToLower()}}}", k.Value));

        }

        private static string UrlWithParameters(string url, IDictionary<string, string> parameters) {

            if (string.IsNullOrEmpty(url)) return "";
            if (parameters == null) return url;
            if (!parameters.Any()) return url;

            var queryString = string.Join("&", parameters.Select(m => $"{m.Key}={m.Value}").ToList());

            return url.IndexOf("?", StringComparison.Ordinal) == -1 ?
                   $"{url}?{queryString}" : $"{url}&{queryString}";

        }

        public async Task<IResponse<TResponse>> GetAsync<TResponse>(string uri) {
            return await RequestAsync<TResponse, string>(new Request<string> {
                EndPoint = new EndPoint {
                    Method = Method.GET,
                    Uri = uri
                }
            });
        }

        public async Task<IResponse<TResponse>> DeleteAsync<TResponse>(string uri) {
            return await RequestAsync<TResponse, string>(new Request<string> {
                EndPoint = new EndPoint {
                    Method = Method.DELETE,
                    Uri = uri
                }
            });
        }

        public string ServiceUrl { get; set; }

        public Task<IResponse<TResponse>> PostAsync<TResponse>(string uri, string data) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = data,
                EndPoint = new EndPoint {
                    Method = Method.POST,
                    Uri = uri
                }
            });
        }

        public Task<IResponse<TResponse>> PutAsync<TResponse>(string uri, string data) {
            return RequestAsync<TResponse, string>(new Request<string> {
                Data = data,
                EndPoint = new EndPoint {
                    Method = Method.PUT,
                    Uri = uri
                }
            });
        }

        public Task<IResponse<TResponse>> PatchAsync<TResponse>(string uri, string data) {
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
                Data = request.Data,
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

                var url = FormatUrl(request.EndPoint.Uri, request.UrlSegment);

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
                        httpResponse = await client.PostAsync(url, content);
                        break;
                    case Method.GET:
                        httpResponse = await client.GetAsync(url);
                        break;
                    case Method.PUT:
                        httpResponse = await client.PutAsync(url, content);
                        break;
                    case Method.DELETE:
                        httpResponse = await client.DeleteAsync(url);
                        break;
                    case Method.PATCH:
                        httpResponse = await client.PatchAsync(url, content);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var statusCode = 520;
                var statusDescription = "Unknown Error";
                var successResult = false;

                if (httpResponse != null) {
                    if (isByteArray) {
                        responseAsBytes = await httpResponse.Content.ReadAsByteArrayAsync();
                    } else {
                        responseString = await httpResponse.Content.ReadAsStringAsync();
                    }
                    statusCode = (int)httpResponse.StatusCode;
                    statusDescription = httpResponse.ReasonPhrase;
                    successResult = httpResponse.IsSuccessStatusCode;
                }

                if (isByteArray) {
                    if (responseAsBytes != null) {
                        response = new Response<TResponse>(responseAsBytes, successResult);
                    }
                } else {
                    if (!string.IsNullOrEmpty(responseString)) {
                        response = new Response<TResponse>(responseString, successResult);
                    }
                }

                response.Status = statusCode;
                response.Description = statusDescription;

            }

            return response;
        }

        private string FormatUrl(string endPoint, IDictionary<string, string> segments) {
            endPoint = endPoint.TrimStart("/"[0])
                               .TrimStart(@"\"[0]);
            if (segments != null && segments.Count > 0) {
                endPoint = segments.Aggregate(endPoint,
                                    (current, val) =>
                                    current.Replace($"{{{val.Key}}}", val.Value));
            }

            return $"{ServiceUrl}/{endPoint}";
        }

    }
}
