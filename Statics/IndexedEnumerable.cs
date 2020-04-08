using System.Collections.Generic;
using System.Linq;

namespace Balsamic
{
    public static class IndexedEnumerable
    {
        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }
    }
}
