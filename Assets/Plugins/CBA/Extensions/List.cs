using System;
using System.Collections.Generic;

namespace CBA.Extensions
{
    public static class List
    {
        public static T Random<T>(this List<T> array)
        {
            if (array.Count == 0)
            {
                throw new ArgumentException("Array length is equal to 0.");
            }

            return array[UnityEngine.Random.Range(0, array.Count)];
        }

        public static T Next<T>(this List<T> array, T current)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].Equals(current))
                {
                    if (i == array.Count - 1)
                    {
                        return array[0];
                    }

                    return array[++i];
                }
            }

            throw new ArgumentException("Current array item is not valid.");
        }

        public static T Previous<T>(this List<T> array, T current)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].Equals(current))
                {
                    if (i == 0)
                    {
                        return Last(array);
                    }

                    return array[--i];
                }
            }

            throw new ArgumentException("Current array item is not valid.");
        }

        public static T First<T>(this List<T> array)
        {
            return array[0];
        }

        public static T Last<T>(this List<T> array)
        {
            return array[array.Count - 1];
        }
        
        public static void Shuffle<T>(this List<T> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}
