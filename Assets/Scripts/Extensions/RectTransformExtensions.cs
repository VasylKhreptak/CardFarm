using UnityEngine;

namespace Extensions
{
    public static class RectTransformExtensions
    {
        public static Vector2 GetAnchoredPositionWithAnchorPreset(this RectTransform rectTransform, Vector2 newAnchorPreset)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            Vector2 currentAnchorPreset = new Vector2(rectTransform.anchorMin.x, rectTransform.anchorMin.y);

            if (currentAnchorPreset == newAnchorPreset)
            {
                return anchoredPosition;
            }

            RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
            Vector2 parentSize = parentTransform.rect.size;

            Vector2 currentAnchorPoint = new Vector2(parentSize.x * currentAnchorPreset.x, parentSize.y * currentAnchorPreset.y);

            Vector2 newAnchorPoint = new Vector2(parentSize.x * newAnchorPreset.x, parentSize.y * newAnchorPreset.y);

            Vector2 anchorOffset = currentAnchorPoint - newAnchorPoint;

            Vector2 newAnchoredPosition = anchoredPosition + anchorOffset;

            return newAnchoredPosition;
        }

        public static Vector2 GetAnchoredPosition(this RectTransform rectTransform, Camera _camera, Vector3 worldPosition)
        {
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(_camera, worldPosition);

            RectTransform parent = rectTransform.parent.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPosition, _camera, out Vector2 anchoredPosition);

            return anchoredPosition;
        }
    }
}
