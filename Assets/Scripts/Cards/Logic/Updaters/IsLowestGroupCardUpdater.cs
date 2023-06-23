using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class IsLowestGroupCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _lowestGroupCardSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
            _lowestGroupCardSubscription = _cardData.LowestGroupCard.Subscribe(OnLowestGroupCardUpdated);
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
