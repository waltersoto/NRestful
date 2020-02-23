using System;
using System.Collections.Generic;
using System.Text;

namespace NRestful.Core.Interfaces
{
    public interface IRequest<TData>
    {
        TData Data { set; get; }
        IDictionary<string, string> Parameters { set; get; }
        IDictionary<string, string> Headers { set; get; }
        IDictionary<string, string> UrlSegment { set; get; }
        IEndPoint EndPoint { set; get; }
    }
    public interface IRequest
    {
        string Data { set; get; }
        IDictionary<string, string> Parameters { set; get; }
        IDictionary<string, string> Headers { set; get; }
        IDictionary<string, string> UrlSegment { set; get; }
        IEndPoint EndPoint { set; get; }
    }
}
