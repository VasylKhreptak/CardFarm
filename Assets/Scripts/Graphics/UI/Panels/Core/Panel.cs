using UnityEngine;

namespace Graphics.UI.Panels.Core
{
    public class Panel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected GameObject _panel;
        
        public void Show()
        {
            _panel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }

        public void Toggle()
        {
            _panel.SetActive(!_panel.activeSelf);
        }
        
        public void SetActive(bool isActive)
        {
            _panel.SetActive(isActive);
        }
    }
}
