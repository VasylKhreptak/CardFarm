namespace CBA.Extensions
{
    public static class Vector3
    {
        public static UnityEngine.Vector3 ReplaceWithByAxes(UnityEngine.Vector3 @in, UnityEngine.Vector3 replaceWith, UnityEngine.Vector3Int axes)
        {
            return UnityEngine.Vector3.Scale(@in, Vector3Int.InverseAxes(axes)) + UnityEngine.Vector3.Scale(replaceWith, axes);
        }

        public static UnityEngine.Vector3 QuadraticLerp(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Vector3 c, float t)
        {
            return UnityEngine.Vector3.Lerp(UnityEngine.Vector3.Lerp(a, b, t), UnityEngine.Vector3.Lerp(b, c, t), t);
        }

        public static UnityEngine.Vector3 CubicLerp(UnityEngine.Vector3 a, UnityEngine.Vector3 b, UnityEngine.Vector3 c, UnityEngine.Vector3 d, float t)
        {
            return UnityEngine.Vector3.Lerp(QuadraticLerp(a, b, c, t), QuadraticLerp(b, c, d, t), t);
        }
    }
}
