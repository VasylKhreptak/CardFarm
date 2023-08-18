using UnityEngine;

namespace CBA.Extensions
{
    public static class Transform
    {
        public static UnityEngine.Transform[] GetChildren(this UnityEngine.Transform transform)
        {
            UnityEngine.Transform[] children = new UnityEngine.Transform[transform.childCount];

            for (int i = 0; i < children.Length; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children;
        }

        public static void DestroyChildren(this UnityEngine.Transform transform)
        {
            UnityEngine.Transform[] children = transform.GetChildren();

            foreach (var child in children)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyImmediateChildren(this UnityEngine.Transform transform)
        {
            UnityEngine.Transform[] children = transform.GetChildren();

            foreach (var child in children)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static void Reset(this UnityEngine.Transform transform)
        {
            ResetPosition(transform);
            ResetRotation(transform);
            ResetScale(transform);
        }

        public static void ResetLocal(this UnityEngine.Transform transform)
        {
            transform.ResetLocalPosition();
            transform.ResetLocalRotation();
            transform.ResetScale();
        }

        public static void ResetPosition(this UnityEngine.Transform transform)
        {
            transform.position = UnityEngine.Vector3.zero;
        }

        public static void ResetLocalPosition(this UnityEngine.Transform transform)
        {
            transform.localPosition = UnityEngine.Vector3.zero;
        }

        public static void ResetRotation(this UnityEngine.Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }

        public static void ResetLocalRotation(this UnityEngine.Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        public static void ResetScale(this UnityEngine.Transform transform)
        {
            transform.localScale = UnityEngine.Vector3.one;
        }
    }
}
