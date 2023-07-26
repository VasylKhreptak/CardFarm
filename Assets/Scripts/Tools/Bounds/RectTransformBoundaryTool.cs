using UnityEngine;

namespace Tools.Bounds
{
    public static class RectTransformBoundaryTool
    {
        public static bool IsInside(this RectTransform innerRectTransform, RectTransform outerRectTransform)
        {
            Vector3[] innerCorners = new Vector3[4];
            Vector3[] outerCorners = new Vector3[4];

            innerRectTransform.GetWorldCorners(innerCorners);
            outerRectTransform.GetWorldCorners(outerCorners);

            for (int i = 0; i < 4; i++)
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(outerRectTransform, innerCorners[i]))
                    return false;
            }

            return true;
        }

        public static Vector3 Clamp(this RectTransform outerRect, RectTransform innerRect)
        {
            return Clamp(outerRect, innerRect, innerRect.position);
        }

        public static Vector3 Clamp(this RectTransform outerRect, RectTransform innerRect, Vector3 innerRectPosition)
        {
            Vector2 innerSize = innerRect.rect.size;
            Vector2 outerSize = outerRect.rect.size;

            Vector3 outerWorldPos = outerRect.position;

            Vector2 outerRectPivot = outerRect.pivot;
            Vector2 innerRectPivot = innerRect.pivot;
            float minX = outerWorldPos.x - outerSize.x * outerRectPivot.x + innerSize.x * innerRectPivot.x;
            float maxX = outerWorldPos.x + outerSize.x * (1f - outerRectPivot.x) - innerSize.x * (1f - innerRectPivot.x);
            float minZ = outerWorldPos.z - outerSize.y * outerRectPivot.y + innerSize.y * innerRectPivot.y;
            float maxZ = outerWorldPos.z + outerSize.y * (1f - outerRectPivot.y) - innerSize.y * (1f - innerRectPivot.y);

            float x = Mathf.Clamp(innerRectPosition.x, minX, maxX);
            float z = Mathf.Clamp(innerRectPosition.z, minZ, maxZ);
            Vector3 clampedPosition = new Vector3(x, innerRectPosition.y, z);

            return clampedPosition;
        }

        public static Vector3 GetRandomPoint(this RectTransform outerRect)
        {
            Vector2 outerSize = outerRect.rect.size;
            Vector3 outerWorldPos = outerRect.position;

            float randomX = Random.Range(outerWorldPos.x - outerSize.x * 0.5f, outerWorldPos.x + outerSize.x * 0.5f);
            float randomZ = Random.Range(outerWorldPos.z - outerSize.y * 0.5f, outerWorldPos.z + outerSize.y * 0.5f);

            Vector3 randomPoint = new Vector3(randomX, outerWorldPos.y, randomZ);

            return randomPoint;
        }

        public static Vector3 GetRandomPoint(this RectTransform outerRect, RectTransform innerRect)
        {
            Vector2 innerSize = innerRect.rect.size;
            Vector2 outerSize = outerRect.rect.size;

            Vector3 outerRectPosition = outerRect.position;
            float minX = outerRectPosition.x - outerSize.x * 0.5f + innerSize.x * 0.5f;
            float maxX = outerRectPosition.x + outerSize.x * 0.5f - innerSize.x * 0.5f;
            float minZ = outerRectPosition.z - outerSize.y * 0.5f + innerSize.y * 0.5f;
            float maxZ = outerRectPosition.z + outerSize.y * 0.5f - innerSize.y * 0.5f;

            Vector3 randomPoint = new Vector3(Random.Range(minX, maxX), innerRect.position.y, Random.Range(minZ, maxZ));

            return randomPoint;
        }
        
        public static void DrawGizmos(this RectTransform outerRect)
        {
            Vector3[] corners = new Vector3[4];
            outerRect.GetWorldCorners(corners);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
        }
    }
}
