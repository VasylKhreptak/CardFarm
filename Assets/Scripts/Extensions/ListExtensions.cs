using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class ListExtensions
    {
        public static bool HasAllElementsOf<T>(this List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count) return false;
            
            HashSet<T> hashSet2 = new HashSet<T>(list2);

            return list1.All(hashSet2.Contains);
        }
    }
}
