namespace CBA.Extensions
{
    public static class Float
    {
        public static bool IsBetween(this float value, float from, float to)
        {
            if (value >= from && value <= to)
            {
                return true;
            }

            return false;
        }

        public static float Clamp01(this float value)
        {
            return UnityEngine.Mathf.Clamp01(value);
        }
    }
}
