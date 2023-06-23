using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Cards.Zones.SellZone.Data;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Zones.SellZone.Logic
{
    public class SellZoneLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SellZoneData _zoneData;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

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
            List<CardData> bottomCards = _zoneData.BottomCards.ToList();

            if (bottomCards.Count == 0) return;

            if (bottomCards.Count >= 1)
            {
                bottomCards[0].UnlinkFromUpper();
            }

            CardData previousSpawnedCoin = null;

            foreach (var bottomCard in bottomCards)
            {
                if (bottomCard.IsSellableCard == false) return;

                SellableCardData sellableCard = bottomCard as SellableCardData;

                if (sellableCard == null) return;

                for (int i = 0; i < sellableCard.Price.Value; i++)
                {
                    CardData spawnedCoin = _cardSpawner.Spawn(Card.Coin, _zoneData.CoinSpawnPoint.position);

                    if (previousSpawnedCoin != null)
                    {
                        spawnedCoin.LinkTo(previousSpawnedCoin);
                    }

                    previousSpawnedCoin = spawnedCoin;
                }

                sellableCard.gameObject.SetActive(false);
            }
        }
    }
}
