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
            _zoneData.Callbacks.onBottomCardsListUpdated += OnCardsUpdated;
        }

        private void StopObserving()
        {
            _zoneData.Callbacks.onBottomCardsListUpdated -= OnCardsUpdated;
        }

        private void OnCardsUpdated()
        {
            if (_zoneData.BottomCards.Count == 0) return;

            int coins = GetCoinsCount();

            SpawnCoins(coins);

            DisableGroupCards();
        }

        private int GetCoinsCount()
        {
            int coinsToSpawn = 0;

            foreach (var card in _zoneData.BottomCards)
            {
                if (card.IsSellableCard)
                {
                    SellableCardData sellableCardData = card as SellableCardData;

                    if (sellableCardData != null)
                    {
                        coinsToSpawn += sellableCardData.Price.Value;
                    }
                }
            }

            return coinsToSpawn;
        }

        private void DisableGroupCards()
        {
            foreach (var card in _zoneData.BottomCards)
            {
                card.gameObject.SetActive(false);
            }
        }

        private void SpawnCoins(int count)
        {
            CardData previousCard = null;

            for (int i = 0; i < count; i++)
            {
                CardData coinCard = _cardSpawner.Spawn(Card.Coin, _zoneData.CoinSpawnPoint.position);

                if (previousCard != null)
                {
                    coinCard.LinkTo(previousCard);
                }

                previousCard = coinCard;
            }
        }
    }
}
