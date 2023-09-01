using Graphics.Animations;
using UnityEngine;

namespace Shop.ShopItems
{
    public class ExclamationMarkHighlighter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FadeHighlighter _fadeHighlighter;
        [SerializeField] private ShopItemBuyEventInvoker _buyEvent;

        private bool _bought = false;

        #region MonoBehaviour

        private void OnEnable()
        {
            if (_bought == false)
            {
                _fadeHighlighter.StartHighlighting();
                _buyEvent.onBought += OnBoughtCard;
            }
        }

        private void OnDisable()
        {
            _fadeHighlighter.StopHighlighting();
            _buyEvent.onBought -= OnBoughtCard;
        }

        #endregion

        private void OnBoughtCard()
        {
            _fadeHighlighter.StopHighlighting();
            _bought = true;
        }
    }
}
