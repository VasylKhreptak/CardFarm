using System;
using Cards.Boosters.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Events
{
    public class BoosterEmptiedEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BoosterCardData _boosterCardData;

        private IDisposable _leftCardsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _boosterCardData = GetComponentInParent<BoosterCardData>(true);
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
            _leftCardsSubscription = _boosterCardData.LeftCards.Subscribe(OnLeftCardsChanged);
        }

        private void StopObserving()
        {
            _leftCardsSubscription?.Dispose();
        }

        private void OnLeftCardsChanged(int leftCards)
        {
            if (leftCards == 0)
            {
                _boosterCardData.BoosterCallabcks.OnEmptied?.Invoke();
            }
        }
    }
}
