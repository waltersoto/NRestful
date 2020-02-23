using NRestful.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRestful.Core
{
    public class EndPoint : IEndPoint
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public Method Method { get; set; }
        public bool Returns { get; set; }
    }
}
