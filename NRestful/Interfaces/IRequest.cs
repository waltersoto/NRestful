using System.Collections.Generic;

namespace NRestful.Interfaces {
    public interface IRequest {
        string Data { set; get; }
        IDictionary<string, string> Parameters { set; get; }
        IDictionary<string, string> Headers { set; get; }
        IDictionary<string, string> UrlSegment { set; get; }
        IEndPoint EndPoint { set; get; }

    }
}
