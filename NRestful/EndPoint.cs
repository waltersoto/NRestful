
using NRestful.Interfaces;

namespace NRestful {
    public class EndPoint : IEndPoint {
        public string Name { get; set; }
        public string Uri { get; set; }
        public Method Method { get; set; }
        public bool Returns { get; set; }
    }
}
