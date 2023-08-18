namespace CBA.Extensions
{
    public static class Bool
    {
        public static int ToDirection(this bool value)
        {
            return value ? 1 : -1;
        }

        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
