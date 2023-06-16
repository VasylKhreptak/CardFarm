using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Returns the random element taking into account the weight of each element;
        /// </summary>
        public static TSource GetByWeight<TSource>(this IEnumerable<TSource> source, Func<TSource, float> weight)
        {
            var enumerable = source as TSource[] ?? source.ToArray();
            float totalWeight = enumerable.Sum(weight);

            float randomNumber = Random.value * totalWeight;

            float weightSum = 0;
            foreach (TSource item in enumerable)
            {
                weightSum += weight(item);
                if (weightSum >= randomNumber)
                {
                    return item;
                }
            }

            throw new InvalidOperationException("Failed to retrieve a random item by weight.");
        }
    }
}
