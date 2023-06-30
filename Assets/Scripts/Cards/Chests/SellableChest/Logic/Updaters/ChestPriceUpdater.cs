﻿using Cards.Chests.SellableChest.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Chests.SellableChest.Logic.Updaters
{
    public class ChestPriceUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestSellableCardData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<ChestSellableCardData>(true);
        }

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.StoredCards.ObserveAdd().Select(x => x.Value.Price.Value)
                .Subscribe(AddToPrice).AddTo(_subscriptions);

            _cardData.StoredCards.ObserveRemove().Select(x => x.Value.Price.Value)
                .Subscribe(RemoveFromPrice).AddTo(_subscriptions);

            _cardData.StoredCards.ObserveReset().Subscribe(_ => ResetPrice()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void AddToPrice(int value)
        {
            _cardData.Price.Value += value;
        }

        private void RemoveFromPrice(int value)
        {
            _cardData.Price.Value -= value;
        }

        private void ResetPrice()
        {
            _cardData.Price.Value = 0;
        }
    }
}
