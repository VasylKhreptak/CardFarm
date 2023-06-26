using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions
{
    public static class MathfExtensions
    {
        public static bool Probability(float probability)
        {
            probability = Mathf.Clamp01(probability);

            if (probability == 0) return false;

            return Random.Range(0f, 1f) <= probability;
        }

        public static void Probability(float probability, Action onTrue, Action onFalse = null)
        {
            if (Probability(probability))
            {
                onTrue?.Invoke();
            }
            else
            {
                onFalse?.Invoke();
            }
        }
    }
}
