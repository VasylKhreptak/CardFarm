using UnityEngine;

namespace Graphics.UI.Panels
{
    public class Panel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panel;
        
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
