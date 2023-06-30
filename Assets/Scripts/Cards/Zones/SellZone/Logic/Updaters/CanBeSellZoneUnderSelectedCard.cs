using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Zones.SellZone.Logic.Updaters
{
    public class CanBeSellZoneUnderSelectedCard : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isCompatibleWithSelectedCardSubscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
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
            _isCompatibleWithSelectedCardSubscription = _cardData.IsCompatibleWithSelectedCard.Subscribe(IsCompatibleValueChanged);
        }

        private void StopObserving()
        {
            _isCompatibleWithSelectedCardSubscription?.Dispose();
        }

        private void IsCompatibleValueChanged(bool isCompatible)
        {
            _cardData.CanBeUnderSelectedCard.Value = isCompatible;
        }
    }
}
