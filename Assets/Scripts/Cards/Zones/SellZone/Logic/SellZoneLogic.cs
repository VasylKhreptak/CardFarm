using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Zones.SellZone.Data;
using Extensions;
using Graphics.UI.Particles.Coins.Logic;
using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic
{
    public class SellZoneLogic : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private SellZoneData _zoneData;

        private CoinsCollector _coinsCollector;
        private Camera _camera;

        [Inject]
        private void Constructor(CoinsCollector coinsCollector, CameraProvider cameraProvider)
        {
            _coinsCollector = coinsCollector;
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _zoneData = GetComponentInParent<SellZoneData>(true);
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
            _zoneData.Callbacks.onBottomCardsListUpdated += OnBottomCardsUpdated;
        }

        private void StopObserving()
        {
            _zoneData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsUpdated;
        }

        private void OnBottomCardsUpdated()
        {
            TrySellBottomCards();
        }

        private void TrySellBottomCards()
        {
            List<CardDataHolder> bottomCards = _zoneData.BottomCards.ToList();

            if (bottomCards.Count == 0) return;

            if (bottomCards.Count >= 1)
            {
                bottomCards[0].UnlinkFromUpper();
            }

            foreach (var bottomCard in bottomCards)
            {
                if (bottomCard.IsSellableCard == false) return;

                if (bottomCard.Card.Value == Card.Coin) return;

                SellableCardData sellableCard = bottomCard as SellableCardData;

                if (sellableCard == null) return;

                Vector3 coinsSpawnPosition = RectTransformUtility.WorldToScreenPoint(_camera, sellableCard.transform.position);

                _coinsCollector.Collect(sellableCard.Price.Value, coinsSpawnPosition);

                sellableCard.gameObject.SetActive(false);
                _zoneData.onSoldCard?.Invoke(sellableCard.Card.Value);
            }
        }
    }
}
