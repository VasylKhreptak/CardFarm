namespace CBA.Extensions
{
    public static class Vector3Int
    {
        public static UnityEngine.Vector3Int InverseAxesRaw(UnityEngine.Vector3Int axes)
        {
            return new UnityEngine.Vector3Int(Mathf.InverseRaw01(axes.x), Mathf.InverseRaw01(axes.y), Mathf.InverseRaw01(axes.z));
        }

        public static UnityEngine.Vector3Int InverseAxes(UnityEngine.Vector3Int axes)
        {
            return new UnityEngine.Vector3Int(Mathf.Inverse01(axes.x), Mathf.Inverse01(axes.y), Mathf.Inverse01(axes.z));
        }
        
        public static UnityEngine.Vector3Int ClampComponents01(this UnityEngine.Vector3Int vector3Int)
        {
            return new UnityEngine.Vector3Int(Mathf.ClampInt01(vector3Int.x), Mathf.ClampInt01(vector3Int.y), Mathf.ClampInt01(vector3Int.z));
        }

        public static UnityEngine.Vector3Int ClampComponents1BothSign(this UnityEngine.Vector3Int vector3Int)
        {
            return new UnityEngine.Vector3Int(Mathf.ClampInt1BothSign(vector3Int.x), Mathf.ClampInt1BothSign(vector3Int.y), Mathf.ClampInt1BothSign(vector3Int.z));
        }
        
        public static UnityEngine.Vector3 ToVector3(UnityEngine.Vector3Int vector3Int)
        {
            return new UnityEngine.Vector3(vector3Int.x, vector3Int.y, vector3Int.z);
        }
    }
}
