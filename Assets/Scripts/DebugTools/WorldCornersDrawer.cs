using UnityEngine;

namespace DebugTools
{
    public class WorldCornersDrawer : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            if (rectTransform == null) return;

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(corners[0], 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(corners[1], 0.5f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(corners[2], 0.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(corners[3], 0.5f);
        }
    }
}
