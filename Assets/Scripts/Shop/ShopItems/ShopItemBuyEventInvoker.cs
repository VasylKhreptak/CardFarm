using System;
using Economy;
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
        private CoinsBank _bank;

        [Inject]
        private void Constructor(CoinsSpender coinsSpender, CoinsBank bank)
        {
            _coinsSpender = coinsSpender;
            _bank = bank;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        #endregion

        private void OnClicked()
        {
            if (_bank.Value < _price) return;

            _coinsSpender.Spend(_price, () => _button.transform.position, onSpentAllCoins: () =>
            {
                onBought?.Invoke();
            });
        }
    }
}
