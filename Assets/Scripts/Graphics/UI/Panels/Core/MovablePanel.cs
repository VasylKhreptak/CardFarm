using UnityEngine;

namespace Graphics.UI.Panels.Core
{
    public class MovablePanel : Panel
    {
        public void SetPosition(Vector3 position)
        {
            _panel.transform.position = position;
        }
    }
}
