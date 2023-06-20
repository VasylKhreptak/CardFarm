using System;
using Cards.CoinChest.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.CoinChest.Graphics.VisualElements
{
    public class CoinsCountText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CoinChestCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _coinsCountSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            StartObservingCoinsCount();
        }

        private void OnDisable()
        {
            StopObservingCoinsCount();
        }

        #endregion

        private void StartObservingCoinsCount()
        {
            StopObservingCoinsCount();
            _coinsCountSubscription = _cardData.Coins.Subscribe(SetCoins);
        }

        private void StopObservingCoinsCount()
        {
            _coinsCountSubscription?.Dispose();
        }

        private void SetCoins(int coins)
        {
            _tmp.text = coins.ToString();
        }
    }
}
