namespace CBA.Extensions
{
    public static class Color
    {
        public static UnityEngine.Color WithAlpha(this UnityEngine.Color color, float alphaValue)
        {
            return new UnityEngine.Color(color.r, color.g, color.b, alphaValue);
        }

        public static UnityEngine.Color WithRed(this UnityEngine.Color color, float red)
        {
            return new UnityEngine.Color(red, color.g, color.b, color.a);
        }

        public static UnityEngine.Color WithGreen(this UnityEngine.Color color, float green)
        {
            return new UnityEngine.Color(color.r, green, color.b, color.a);
        }

        public static UnityEngine.Color WithBlue(this UnityEngine.Color color, float blue)
        {
            return new UnityEngine.Color(color.r, color.g, blue, color.a);
        }
    }
}
