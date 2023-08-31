using System;
using Graphics.UI.Particles.Coins.Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop.ShopItems
{
    public class ShopItemBuyEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        [Header("Preferences")]
        [SerializeField] private int _price = 20;

        public event Action onBought;

        private CoinsSpender _coinsSpender;

        [Inject]
        private void Constructor(CoinsSpender coinsSpender)
        {
            _coinsSpender = coinsSpender;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        #endregion

        private void OnClicked()
        {
            _coinsSpender.Spend(_price, () => _button.transform.position, onSpentAllCoins: () =>
            {
                onBought?.Invoke();
            });
        }
    }
}
