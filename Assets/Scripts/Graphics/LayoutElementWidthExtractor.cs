using UnityEngine;
using UnityEngine.UI;

namespace Graphics
{
    public class LayoutElementWidthExtractor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private RectTransform[] _rectTransforms;

        #region MonoBehaviour

        private void Update()
        {
            UpdateWidth();
        }

        #endregion

        private void UpdateWidth()
        {
            float width = 0;

            for (int i = 0; i < _rectTransforms.Length; i++)
            {
                float rectWidth = _rectTransforms[i].rect.width;

                if (rectWidth > width)
                {
                    width = rectWidth;
                }
            }

            _layoutElement.preferredWidth = width;
        }
    }
}
