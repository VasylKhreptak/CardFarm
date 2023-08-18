using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CBA.Extensions
{
    public static class Mathf
    {
        public static int ClampInt(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }

            return value < min ? min : value;
        }

        public static int ClampInt01(int value)
        {
            return ClampInt(value, 0, 1);
        }

        public static int ClampInt1BothSign(int value)
        {
            return ClampInt(value, -1, 1);
        }

        public static int InverseRaw01(int value)
        {
            return Inverse01(ClampInt01(value));
        }

        public static int Inverse01(int value)
        {
            if (value != 0 && value != 1)
            {
                Debug.LogWarning("Inverse01 can only be used on 0 or 1");
            }

            return value == 0 ? 1 : 0;
        }

        public static bool Probability(float probability)
        {
            if (probability == 0) return false;
            if (probability == 1) return true;

            if (probability.IsBetween(0f, 1f) == false)
            {
                Debug.LogWarning("Probability must be between 0 and 1");
                probability = probability.Clamp01();
            }

            return Random.value <= probability;
        }

        public static void Probability(float probability, Action action)
        {
            if (Probability(probability))
            {
                action.Invoke();
            }
        }
    }
}
