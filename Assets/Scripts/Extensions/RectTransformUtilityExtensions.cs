using UnityEngine;

namespace Extensions
{
    public static class RectTransformUtilityExtensions
    {
        public static Vector3 ProjectPointOnCameraCanvas(Canvas canvas, RectTransform canvasRect, Vector3 point)
        {
            Camera camera = canvas.worldCamera;

            Plane plane = new Plane(canvas.transform.forward, canvasRect.position);

            Ray ray = new Ray(point, camera.transform.position - point);

            plane.Raycast(ray, out float distance);

            return ray.GetPoint(distance);
        }
    }
}
