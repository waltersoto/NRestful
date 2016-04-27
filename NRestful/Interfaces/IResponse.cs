
namespace NRestful.Interfaces {
    public interface IResponse<T> {
        T Content { set; get; }
        bool Success { set; get; }
        int Status { set; get; }
        string Description { set; get; }
    }
}
