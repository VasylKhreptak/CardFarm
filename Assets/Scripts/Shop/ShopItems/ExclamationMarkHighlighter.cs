using Graphics.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.ShopItems
{
    public class ExclamationMarkHighlighter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FadeHighlighter _fadeHighlighter;
        [SerializeField] private Button _buyButton;

        private bool _bought = false;

        #region MonoBehaviour

        private void OnEnable()
        {
            if (_bought == false)
            {
                _fadeHighlighter.StartHighlighting();
                _buyButton.onClick.AddListener(OnClick);
            }
        }

        private void OnDisable()
        {
            _fadeHighlighter.StopHighlighting();

            _buyButton.onClick.RemoveListener(OnClick);
        }

        #endregion

        private void OnClick()
        {
            _fadeHighlighter.StopHighlighting();
            _bought = true;
        }
    }
}
