using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Events
{
    public class OnClickedAnyGroupCardInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            _cardData.Callbacks.onClicked += OnClicked;
        }

        private void StopObserving()
        {
            _cardData.Callbacks.onClicked -= OnClicked;
        }

        private void OnClicked()
        {
            InvokeEvent();
        }

        private void InvokeEvent()
        {
            foreach (var groupCard in _cardData.GroupCards.ToList())
            {
                groupCard.Callbacks.onClickedAnyGroupCard?.Invoke();
            }
        }
    }
}
