using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRestful.Core {

    public static class Extensions {


        public static byte[] ObjectToSerializedByteArray<T>(this T objectToSerialize) {
            var json = JsonConvert.SerializeObject(objectToSerialize);
            if (json == null) return Array.Empty<byte>();
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
