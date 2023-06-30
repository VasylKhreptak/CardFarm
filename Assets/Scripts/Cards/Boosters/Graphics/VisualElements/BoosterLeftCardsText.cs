using System;
using Cards.Boosters.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Graphics.VisualElements
{
    public class BoosterLeftCardsText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BoosterCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _leftCardsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<BoosterCardData>(true);
        }

        private void OnEnable()
        {
            StartObservingLeftCards();
        }

        private void OnDisable()
        {
            StopObservingLeftCards();
        }

        #endregion

        private void StartObservingLeftCards()
        {
            StopObservingLeftCards();
            _leftCardsSubscription = _cardData.LeftCards.Subscribe(OnLeftCardsChanged);
        }

        private void StopObservingLeftCards()
        {
            _leftCardsSubscription?.Dispose();
        }

        private void OnLeftCardsChanged(int leftCards)
        {
            _tmp.text = leftCards.ToString();
        }
    }
}
