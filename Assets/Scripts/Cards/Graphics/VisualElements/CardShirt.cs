using UnityEngine;
using UnityEngine.UI;

namespace Cards.Graphics.VisualElements
{
    public class CardShirt : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

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
        }

        public void Hide()
        {
            _image.enabled = false;
            transform.SetAsLastSibling();
        }

        public void SetState(bool enabled)
        {
            _image.enabled = enabled;
            transform.SetAsLastSibling();
        }
    }
}
