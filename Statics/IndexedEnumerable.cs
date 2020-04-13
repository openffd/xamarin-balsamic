using System.Collections.Generic;
using System.Linq;

namespace Balsamic
{
    static class IndexedEnumerable
    {
        internal static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }
    }
}
