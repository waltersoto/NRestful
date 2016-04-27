
using System.Collections.Generic;
using NRestful.Interfaces;

namespace NRestful {
    public class Request : IRequest {
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
