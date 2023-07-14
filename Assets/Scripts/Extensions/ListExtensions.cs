using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class ListExtensions
    {
        public static bool HasExactlyAllElementsOf<T>(this List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count) return false;

            foreach (T element in list1)
            {
                int countInList1 = list1.Count(x => EqualityComparer<T>.Default.Equals(x, element));
                int countInList2 = list2.Count(x => EqualityComparer<T>.Default.Equals(x, element));

                if (countInList1 != countInList2)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasAllLessElementOf<T>(this List<T> list1, List<T> list2)
        {
            foreach (T element in list2)
            {
                int countInList1 = list1.Count(x => EqualityComparer<T>.Default.Equals(x, element));
                int countInList2 = list2.Count(x => EqualityComparer<T>.Default.Equals(x, element));

                if (countInList2 > countInList1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
