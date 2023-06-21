using System.Collections.Generic;
using Cards.Chests.SellableChest.Data;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using Table.CardSelectors;
using Table.Core;
using UnityEngine;
using Zenject;

namespace Coins
{
    public class CoinsProvider : MonoBehaviour
    {
        private CardsTable _cardsTable;
        private CoinChestSelector _coinChestSelector;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardsTable cardsTable, CoinChestSelector coinChestSelector, CardSpawner cardSpawner)
        {
            _cardsTable = cardsTable;
            _coinChestSelector = coinChestSelector;
            _cardSpawner = cardSpawner;
        }

        public int GetCoinsCount() => _cardsTable.GetCardsCount(Card.Coin) + GetChestCoinsCount();

        public int GetChestCoinsCount()
        {
            int count = 0;

            foreach (var coinChestCard in _coinChestSelector.SelectedCards)
            {
                count += (coinChestCard as ChestSellableCardData).StoredCards.Count;
            }

            return count;
        }

        public bool TryGetCoinFromChests(out CardData cardData)
        {
            foreach (var card in _coinChestSelector.SelectedCards)
            {
                ChestSellableCardData chest = card as ChestSellableCardData;

                if (chest.StoredCards.Count > 0)
                {
                    chest.StoredCards.RemoveAt(0);
                    cardData = _cardSpawner.Spawn(Card.Coin, chest.transform.position);
                    return true;
                }
            }

            cardData = null;
            return false;
        }

        public bool TryGetCoin(out CardData cardData)
        {
            bool hasCoinsInTable = _cardsTable.TryGetLowestGroupCardOrFirst(Card.Coin, out CardData coinFromTable);

            if (hasCoinsInTable)
            {
                coinFromTable.Separate();
                cardData = coinFromTable;
                return true;
            }

            bool hasCoinsInChests = TryGetCoinFromChests(out CardData coinFromChest);

            if (hasCoinsInChests)
            {
                coinFromTable.Separate();
                cardData = coinFromChest;
                return true;
            }

            cardData = null;
            return false;
        }

        public bool TryGetCoins(int count, out List<CardData> coins)
        {
            List<CardData> foundCoins = new List<CardData>();

            int coinsCount = GetCoinsCount();

            if (count > coinsCount)
            {
                coins = foundCoins;
                return false;
            }

            CardData[] coinBuffer = new CardData[count];
            int coinsCountInTable = _cardsTable.TryGetLowestGroupCards(Card.Coin, ref coinBuffer);

            for (int i = 0; i < coinsCountInTable; i++)
            {
                coinBuffer[i].Separate();
                foundCoins.Add(coinBuffer[i]);
            }

            if (coinsCountInTable == count)
            {
                coins = foundCoins;
                return true;
            }

            int leftCoinsCount = count - coinsCountInTable;

            if (leftCoinsCount > 0)
            {
                foreach (var coinChests in _coinChestSelector.SelectedCards)
                {
                    ChestSellableCardData chest = coinChests as ChestSellableCardData;

                    int chestSize = chest.StoredCards.Count;

                    for (int i = 0; i < chestSize; i++)
                    {
                        chest.StoredCards.RemoveAt(0);
                        CardData coin = _cardSpawner.Spawn(Card.Coin, chest.transform.position);
                        foundCoins.Add(coin);

                        if (foundCoins.Count == count) break;
                    }

                    if (foundCoins.Count == count)
                    {
                        coins = foundCoins;
                        return true;
                    }
                }
            }

            coins = foundCoins;
            return false;
        }
    }
}
