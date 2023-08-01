using System.Collections.Generic;
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

        public static Vector3 Clamp(this RectTransform outerRect, Vector3 point)
        {
            Vector2 outerSize = outerRect.rect.size;
            Vector3 outerWorldPos = outerRect.position;

            float minX = outerWorldPos.x - outerSize.x * 0.5f;
            float maxX = outerWorldPos.x + outerSize.x * 0.5f;
            float minZ = outerWorldPos.z - outerSize.y * 0.5f;
            float maxZ = outerWorldPos.z + outerSize.y * 0.5f;

            float x = Mathf.Clamp(point.x, minX, maxX);
            float z = Mathf.Clamp(point.z, minZ, maxZ);
            Vector3 clampedPosition = new Vector3(x, point.y, z);

            return clampedPosition;
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

        public static bool IsOverlapping(this RectTransform rectTransform1, RectTransform rectTransform2)
        {
            Rect rect1 = new Rect(rectTransform1.localPosition.x, rectTransform1.localPosition.y, rectTransform1.rect.width, rectTransform1.rect.height);
            Rect rect2 = new Rect(rectTransform2.localPosition.x, rectTransform2.localPosition.y, rectTransform2.rect.width, rectTransform2.rect.height);

            return rect1.Overlaps(rect2);
        }

        public static bool IsOverlapping(this RectTransform rectTransform1, List<RectTransform> rectTransform2)
        {
            for (int i = 0; i < rectTransform2.Count; i++)
            {
                if (rectTransform1.IsOverlapping(rectTransform2[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector3 GetClosestRandomPoint(this RectTransform outerRect, List<RectTransform> innerRects, RectTransform targetRect, Vector3 origin)
        {
            const int IterationCount = 300;
            const float MinOriginDistance = 1f;
            (Vector3 position, float weight)[] points = new (Vector3 position, float weight)[IterationCount];

            Vector3 initialRectPosition = targetRect.position;

            (Vector3 position, float weight) foundPoint = (Vector3.zero, float.MaxValue);

            for (int i = 0; i < IterationCount; i++)
            {
                Vector3 randomPoint = outerRect.GetRandomPoint(targetRect);

                float originDistance = Vector3.Distance(origin, randomPoint);
                originDistance = Mathf.Max(originDistance, MinOriginDistance);
                targetRect.position = randomPoint;

                int intersectCount = 0;

                for (int j = 0; j < innerRects.Count; j++)
                {
                    var innerRect = innerRects[j];
                    if (innerRect.IsOverlapping(targetRect))
                    {
                        intersectCount++;
                    }
                }

                float intersectWeight;

                intersectWeight = intersectCount == 0 ? 0.01f : intersectCount * intersectCount;

                points[i] = (randomPoint, originDistance * intersectWeight);

                if (points[i].weight < foundPoint.weight)
                {
                    foundPoint = points[i];
                }
            }

            targetRect.position = initialRectPosition;

            return foundPoint.position;
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
