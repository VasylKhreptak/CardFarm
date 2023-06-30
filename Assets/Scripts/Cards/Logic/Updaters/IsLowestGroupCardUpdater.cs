using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class IsLowestGroupCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _lowestGroupCardSubscription;

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
