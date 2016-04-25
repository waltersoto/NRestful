
namespace NRestful {
    public interface IEndPoint {

        string Name { set; get; }
        string Uri { set; get; }
        Method Method { set; get; }
        bool Returns { set; get; }
    }
}
