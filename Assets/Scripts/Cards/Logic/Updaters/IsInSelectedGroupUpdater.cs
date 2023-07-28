using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsInSelectedGroupUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _firstGroupCardSubscription;
        private IDisposable _isCardSelectedSubscription;

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
            StartObservingFirstGroupCard();
        }

        private void OnDisable()
        {
            StopObservingFirstGroupCard();
            StopObservingIfCardSelected();
        }

        #endregion

        private void StartObservingFirstGroupCard()
        {
            StopObservingFirstGroupCard();

            _firstGroupCardSubscription = _cardData.FirstGroupCard.Subscribe(OnFirstGroupCardChanged);
        }

        private void StopObservingFirstGroupCard()
        {
            _firstGroupCardSubscription?.Dispose();
        }

        private void OnFirstGroupCardChanged(CardData firstGroupCard)
        {
            StopObservingFirstGroupCard();

            if (firstGroupCard != null)
            {
                StartObservingIfCardSelected(firstGroupCard);
            }
        }

        private void StartObservingIfCardSelected(CardData card)
        {
            StopObservingIfCardSelected();

            _isCardSelectedSubscription = card.IsSelected.Subscribe(isSelected =>
            {
                _cardData.IsInSelectedGroup.Value = isSelected;
            });
        }

        private void StopObservingIfCardSelected()
        {
            _isCardSelectedSubscription?.Dispose();
            _cardData.IsInSelectedGroup.Value = false;
        }
    }
}
