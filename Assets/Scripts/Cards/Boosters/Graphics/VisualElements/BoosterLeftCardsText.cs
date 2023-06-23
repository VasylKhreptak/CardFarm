using System;
using Cards.Boosters.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Boosters.Graphics.VisualElements
{
    public class BoosterLeftCardsText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoosterCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _leftCardsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
            _cardData ??= GetComponentInParent<BoosterCardData>();
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
