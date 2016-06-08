using System;
using System.Linq;

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

            return types.Any(x => t == x);

        }

    }
}
