using UnityEngine;

namespace Graphics.UI.Panels.Core
{
    public class MovablePanel : Panel
    {
        public void SetPosition(Vector3 position)
        {
            _panel.transform.position = position;
        }

        public void SetAnchoredPosition3D(Vector3 anchoredPosition3D)
        {
            _rectTransform.anchoredPosition3D = anchoredPosition3D;
        }
    }
}
