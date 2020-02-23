using System;
using System.Collections.Generic;
using System.Text;

namespace NRestful.Core.Interfaces
{
    public interface IEndPoint
    {
        string Name { set; get; }
        Uri Uri { set; get; }
        Method Method { set; get; }
        bool Returns { set; get; }
    }
}
