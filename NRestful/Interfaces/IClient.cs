
using System.Threading.Tasks;

namespace NRestful.Interfaces {
    public interface IClient {
        Task<IResponse<TResponse>> RequestAsync<TResponse>(IRequest request);
        Task<IResponse<TResponse>> GetAsync<TResponse>(string uri);
        Task<IResponse<TResponse>> PostAsync<TResponse>(string uri, string data);
        Task<IResponse<TResponse>> PutAsync<TResponse>(string uri, string data);
        Task<IResponse<TResponse>> PatchAsync<TResponse>(string uri, string data);
        Task<IResponse<TResponse>> DeleteAsync<TResponse>(string uri);
        string ServiceUrl { set; get; }

    }
}
