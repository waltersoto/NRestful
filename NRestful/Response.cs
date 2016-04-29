using System;
using NRestful.Interfaces;

namespace NRestful {
    public class Response<T> : IResponse<T> {
        public Response() {
            Success = false;
        }

        public Response(string content, bool success) {
            Success = success;
            if (!Success) return;

            if (typeof(T).IsPrimitive()) {
                Content = (T)Convert.ChangeType(content, typeof(T));
                return;
            }

            Content = JsonHelper.FromJson<T>(content);
        }

        public T Content { set; get; }
        public bool Success { set; get; }
        public int Status { set; get; }
        public string Description { set; get; }

    }

}
