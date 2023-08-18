using System;

namespace CBA.Extensions
{
    public static class Array
    {
        public static T Random<T>(this T[] array)
        {
            if (array.Length == 0)
            {
                throw new ArgumentException("Array length is equal to 0.");
            }

            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T Next<T>(this T[] array, T current)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(current))
                {
                    if (i == array.Length - 1)
                    {
                        return array[0];
                    }

                    return array[++i];
                }
            }

            throw new ArgumentException("Current array item is not valid.");
        }

        public static T Previous<T>(this T[] array, T current)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(current))
                {
                    if (i == 0)
                    {
                        return array.Last();
                    }

                    return array[--i];
                }
            }

            throw new ArgumentException("Current array item is not valid.");
        }

        public static T First<T>(this T[] array)
        {
            return array[0];
        }

        public static T Last<T>(this T[] array)
        {
            return array[array.Length - 1];
        }

        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}
