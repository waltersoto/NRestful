using NRestful.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRestful.Core
{
    public class Request<TData> : IRequest<TData>
    {
        public Request() {
            Parameters = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
            UrlSegment = new Dictionary<string, string>();
        }
        public TData Data { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> UrlSegment { get; set; }
        public IEndPoint EndPoint { get; set; }
    }

    public class Request : IRequest
    {
        public Request() {
            Parameters = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
            UrlSegment = new Dictionary<string, string>();
        }
        public string Data { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> UrlSegment { get; set; }
        public IEndPoint EndPoint { get; set; }
    }
}
