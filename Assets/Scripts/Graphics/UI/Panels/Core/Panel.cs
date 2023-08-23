using UnityEngine;

namespace Graphics.UI.Panels.Core
{
    public class Panel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected GameObject _panel;

        protected RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = _panel.GetComponent<RectTransform>();

                return _rectTransform;
            }
        }

        public virtual void Show()
        {
            if (_panel == null) return;

            _panel.SetActive(true);
        }

        public virtual void Hide()
        {
            if (_panel == null) return;

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
