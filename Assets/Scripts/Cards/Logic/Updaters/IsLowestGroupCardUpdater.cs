using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsLowestGroupCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _lowestGroupCardSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
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
            _lowestGroupCardSubscription = _cardData.LastGroupCard.Subscribe(OnLowestGroupCardUpdated);
        }

        private void StopObserving()
        {
            _lowestGroupCardSubscription?.Dispose();
        }

        private void OnLowestGroupCardUpdated(CardData cardData)
        {
            _cardData.IsLowestGroupCard.Value = cardData == _cardData;
        }
    }
}
