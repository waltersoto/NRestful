using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NRestful {
    public static class PrimitiveExtension {

        public static byte[] ObjectToSerializedByteArray<T>(this T obj) {
            var json = JsonConvert.SerializeObject(obj);
            if (json == null) return new byte[] { };
            var charArray = json.ToCharArray();
            var bytes = new byte[charArray.Length * sizeof(char)];
            Buffer.BlockCopy(charArray, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static bool IsPrimitive(this Type t) {
            var types = new[]
                         {
                              typeof (Enum),
                              typeof (string),
                              typeof (char),
                              typeof (Guid),
                              typeof (bool),
                              typeof (byte),
                              typeof(short),
                              typeof (int),
                              typeof (long),
                              typeof (float),
                              typeof (double),
                              typeof (decimal),
                              typeof (sbyte),
                              typeof (ushort),
                              typeof (uint),
                              typeof (ulong),
                              typeof (DateTime),
                              typeof (DateTimeOffset),
                              typeof (TimeSpan),
                          };

            return types.Any(x => t == x);

        }

    }
}
