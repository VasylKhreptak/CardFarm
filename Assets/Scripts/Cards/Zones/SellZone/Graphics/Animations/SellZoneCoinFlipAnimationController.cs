using System;
using Cards.Zones.SellZone.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Graphics.Animations
{
    public class SellZoneCoinFlipAnimationController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _sellZoneData;

        private IDisposable _totalCoinsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _sellZoneData = GetComponentInParent<SellZoneData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _totalCoinsSubscription = _sellZoneData.SelectedCardsTotalPrice.Subscribe(OnSelectedCardsTotalPriceChanged);
        }

        private void StopObserving()
        {
            _totalCoinsSubscription?.Dispose();
        }

        private void OnSelectedCardsTotalPriceChanged(int totalCoins)
        {
            if (totalCoins > 0)
            {
                _sellZoneData.CoinFlipAnimation.StartFlipping();
            }
            else
            {
                _sellZoneData.CoinFlipAnimation.StopFlipping();
            }
        }
    }
}
