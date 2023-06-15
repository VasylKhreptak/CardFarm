using System;
using System.Collections.Generic;
using Cards.Data;
using Extensions.Cards;
using UniRx;
using UnityEngine;

namespace Cards.Graphics
{
    public class CardLayerOrderUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _subscription;

        #region MonoBehaviour

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
                .CombineLatest(_cardData.IsSelectedCard, _cardData.IsTopCard, _cardData.IsSingleCard)
                .Where(list => list[0] && (list[1] || list[2]))
                .Subscribe(_ => UpdateLayers());

        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateLayers()
        {
            List<CardData> groupCards = _cardData.FindGroupCards();

            foreach (var card in groupCards)
            {
                card.transform.SetAsLastSibling();
            }
        }
    }
}
