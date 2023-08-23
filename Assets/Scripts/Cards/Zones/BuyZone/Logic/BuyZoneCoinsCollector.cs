﻿using Cards.Zones.BuyZone.Data;
using Economy;
using Graphics.UI.Particles.Coins.Logic;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneCoinsCollector : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;

        private bool _canCollectCoins = true;

        private CoinsSpender _coinsSpender;
        private CoinsBank _coinsBank;

        [Inject]
        private void Constructor(CoinsSpender coinsSpender,
            CoinsBank coinsBank,
            CanvasProvider canvasProvider)
        {
            _coinsSpender = coinsSpender;
            _coinsBank = coinsBank;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<BuyZoneData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _canCollectCoins = true;
        }

        #endregion

        private void StartObserving()
        {
            StartObservingClick();
        }

        private void StopObserving()
        {
            StopObservingClick();
        }

        private void StartObservingClick()
        {
            StopObservingClick();
            _cardData.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingClick()
        {
            _cardData.Callbacks.onClicked -= OnClicked;
        }

        private void OnClicked()
        {
            CollectCoins();
        }

        private void CollectCoins()
        {
            if (_canCollectCoins == false) return;

            int totalCoinsCount = _coinsBank.Value;

            int coinsToSpawn = Mathf.Min(_cardData.LeftCoins.Value, totalCoinsCount);

            if (coinsToSpawn == 0) return;

            _coinsSpender.Spend(coinsToSpawn, () => _cardData.transform.position,
                () =>
                {
                    _cardData.CollectedCoins.Value += 1;
                },
                () =>
                {
                    _canCollectCoins = true;
                });

            _canCollectCoins = false;
        }
    }
}
