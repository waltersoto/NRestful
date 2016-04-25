
using Newtonsoft.Json;

namespace NRestful {
    public static class JsonHelper {

        public static string ToJson<T>(T items) {
            return JsonConvert.SerializeObject(items);
        }

        public static T FromJson<T>(string txt) {
            return JsonConvert.DeserializeObject<T>(txt);
        }


    }
}
