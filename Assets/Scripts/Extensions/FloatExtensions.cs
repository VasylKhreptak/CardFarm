namespace Extensions
{
    public static class FloatExtensions
    {
        public static bool IsBetween(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }
    }
}
