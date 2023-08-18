using UnityEngine;

namespace CBA.Extensions
{
    public static class Gizmos
    {
        public static void DrawArrow(UnityEngine.Vector3 position, UnityEngine.Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            if (direction == UnityEngine.Vector3.zero) return;

            UnityEngine.Gizmos.DrawRay(position, direction);

            UnityEngine.Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new UnityEngine.Vector3(0, 0, 1);
            UnityEngine.Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new UnityEngine.Vector3(0, 0, 1);
            UnityEngine.Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * new UnityEngine.Vector3(0, 0, 1);
            UnityEngine.Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new UnityEngine.Vector3(0, 0, 1);

            UnityEngine.Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            UnityEngine.Gizmos.DrawRay(position + direction, left * arrowHeadLength);
            UnityEngine.Gizmos.DrawRay(position + direction, up * arrowHeadLength);
            UnityEngine.Gizmos.DrawRay(position + direction, down * arrowHeadLength);
        }

        public static void DrawPoints(UnityEngine.Vector3[] points, float radius)
        {
            foreach (var point in points)
            {
                UnityEngine.Gizmos.DrawSphere(point, radius);
            }
        }

        public static void DrawLines(UnityEngine.Vector3[] points)
        {
            for (int i = 1; i < points.Length; i++)
            {
                UnityEngine.Vector3 startPoint = points[i - 1];
                UnityEngine.Vector3 endPoint = points[i];
                UnityEngine.Gizmos.DrawLine(startPoint, endPoint);
            }
        }

#if UNITY_EDITOR

        public static void DrawAngle(UnityEngine.Vector3 center, UnityEngine.Vector3 from, UnityEngine.Vector3 to, float radius,
            UnityEngine.Color arcColor, UnityEngine.Color wireArcColor)
        {
            float angle = UnityEngine.Vector3.Angle(from, to);

            if (UnityEngine.Mathf.Approximately(angle, 0)) return;

            UnityEngine.Vector3 arcNormal = UnityEngine.Vector3.Cross(from, to).normalized;
            UnityEngine.Vector3 fromEndPoint = center + (from * radius);
            UnityEngine.Vector3 toEndPoint = center + (to * radius);
            float endPointSize = radius / 50f;
            UnityEngine.Vector3 secondArrowPoint = center + ((Quaternion.AngleAxis(-5f, arcNormal) * to) * radius);

            UnityEngine.Gizmos.DrawSphere(fromEndPoint, endPointSize);
            UnityEditor.Handles.color = arcColor;
            UnityEditor.Handles.DrawSolidArc(center, arcNormal, from, angle, radius);
            UnityEditor.Handles.color = wireArcColor;
            UnityEditor.Handles.DrawWireArc(center, arcNormal, from, angle, radius);
            UnityEngine.Gizmos.color = wireArcColor;
            DrawArrow(toEndPoint, toEndPoint - secondArrowPoint);
        }

#endif

    }
}
