using System;
using System.Collections.Generic;
using System.Linq;

namespace Songbird.Web.Extensions {
    public static class EnumerableExtensions {
        public static bool None<T>(this IEnumerable<T> collection) => !collection.Any();
        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> predicate) => !collection.Any(predicate);
    }
}
