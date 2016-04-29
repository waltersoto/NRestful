using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRestful {
    public static class PrimitiveExtension {

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
            var tt = types.Any(x => t == x);
            return types.Any(x => t == x);

        }

    }
}
