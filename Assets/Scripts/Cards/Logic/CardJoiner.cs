using System;
using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class CardJoiner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _subscription;

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

            _subscription = Observable
                .CombineLatest(_cardData.IsSelectedCard, _cardData.JoinableCard,
                    (isSelected, targetCard) => new { isSelected, targetCard })
                .Subscribe(tuple =>
                {
                    OnCardDataUpdated(tuple.isSelected, tuple.targetCard);
                });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnCardDataUpdated(bool isSelected, CardData targetCard)
        {
            if (isSelected == false && targetCard != null)
            {
                _cardData.LinkTo(targetCard);
            }
        }
    }
}
