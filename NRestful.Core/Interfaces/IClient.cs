using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRestful.Core.Interfaces
{

    public interface IClient
    {
        Task<IResponse<TResponse>> RequestAsync<TResponse>(IRequest request);
        Task<IResponse<TResponse>> RequestAsync<TResponse, TRequestData>(IRequest<TRequestData> request);
        Task<IResponse<TResponse>> GetAsync<TResponse>(Uri uri);
        Task<IResponse<TResponse>> PostAsync<TResponse>(Uri uri, string data);
        Task<IResponse<TResponse>> PutAsync<TResponse>(Uri uri, string data);
        Task<IResponse<TResponse>> PatchAsync<TResponse>(Uri uri, string data);
        Task<IResponse<TResponse>> DeleteAsync<TResponse>(Uri uri);
        Uri ServiceUrl { set; get; }

    }

}
