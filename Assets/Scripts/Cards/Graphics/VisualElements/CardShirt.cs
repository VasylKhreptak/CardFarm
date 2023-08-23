using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Graphics.VisualElements
{
    public class CardShirt : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

        private BoolReactiveProperty _isEnabled = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;

        #region MonoBehaviour

        private void OnValidate()
        {
            _image = GetComponent<Image>();
        }

        private void Awake()
        {
            Hide();
        }

        #endregion

        public void Show()
        {
            _image.enabled = true;
            transform.SetAsLastSibling();
            _isEnabled.Value = true;
        }

        public void Hide()
        {
            _image.enabled = false;
            transform.SetAsLastSibling();
            _isEnabled.Value = false;
        }

        public void SetState(bool enabled)
        {
            _image.enabled = enabled;
            transform.SetAsLastSibling();
            _isEnabled.Value = enabled;
        }
    }
}
