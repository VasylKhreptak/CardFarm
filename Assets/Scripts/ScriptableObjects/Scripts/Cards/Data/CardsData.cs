using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using Cards.Food.Data;
using Cards.Tags;
using Data.Cards;
using Data.Cards.Core;
using Extensions;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.UI;
using DamageableCardData = Cards.Data.DamageableCardData;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace ScriptableObjects.Scripts.Cards.Data
{
    [CreateAssetMenu(fileName = "CardsData", menuName = "ScriptableObjects/CardsData")]
    public class CardsData : ScriptableObject
    {
        [Header("Data")]
        [SerializeReference] private List<CardData> _cardsData = new List<CardData>();

        private Dictionary<Card, CardDataHolder> _cards;

        public bool TryGetValue(Card card, out CardDataHolder cardData)
        {
            if (_cards == null || _cards.Count == 0)
            {
                InitData();
            }

            return _cards.TryGetValue(card, out cardData);
        }

        private void InitData()
        {
            _cards = new Dictionary<Card, CardDataHolder>();

            foreach (var cardData in _cardsData)
            {
                _cards.Add(cardData.Key, cardData.Value);
            }
        }

        [Serializable]
        public class CardData
        {
            public Card Key;
            [SerializeReference] public CardDataHolder Value;
        }

        [Button()] private void AddCardData()
        {
            _cardsData.Add(new CardData
            {
                Key = default,
                Value = new CardDataHolder()
            });
        }

        [Button()] private void AddSellableCardData()
        {
            _cardsData.Add(new CardData
            {
                Key = default,
                Value = new SellableCardDataHolder()
            });
        }

        [Button()] private void AddDamageableCardData()
        {
            _cardsData.Add(new CardData
            {
                Key = default,
                Value = new DamageableCardDataHolder()
            });
        }

        [Button()] private void AddFoodCardData()
        {
            _cardsData.Add(new CardData
            {
                Key = default,
                Value = new FoodCardDataHolder()
            });
        }

        [Button()]
        private void ExecuteAction()
        {
            Debug.Log("Action Executed");
        }

        #region Editor

        [Button()]
        private void SyncWithPrefabs()
        {
            _cardsData.Clear();

            string[] prefabPaths = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs" });

            List<global::Cards.Data.CardData> cards = new List<global::Cards.Data.CardData>();

            foreach (string prefabGUID in prefabPaths)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));

                if (prefab.TryGetComponent(out global::Cards.Data.CardData cardData))
                {
                    if (cardData.Name.Value != "Name")
                    {
                        cards.Add(cardData);
                        AddCardData(cardData);
                    }
                }
            }

            Debug.Log($"Found Cards {cards.Count}");
        }

        private void AddCardData(global::Cards.Data.CardData cardData)
        {
            CardDataHolder cardDataHolder = new CardDataHolder();
            Card key = cardData.Card.Value;

            if (cardData.IsFood)
            {
                cardDataHolder = CreateFoodCardDataHolder(cardData);
            }
            else if (cardData.IsSellableCard)
            {
                cardDataHolder = CreateSellableCardDataHolder(cardData);
            }
            else if (cardData.IsDamageable)
            {
                cardDataHolder = CreateDamageableCardDataHolder(cardData);
            }
            else
            {
                cardDataHolder = CreateBaseCardDataHolder(cardData);
            }

            _cardsData.Add(new CardData()
            {
                Key = key,
                Value = cardDataHolder
            });
        }

        private SellableCardDataHolder CreateSellableCardDataHolder(global::Cards.Data.CardData cardData)
        {
            SellableCardDataHolder sellableCardDataHolder = new SellableCardDataHolder(CreateBaseCardDataHolder(cardData));

            if (cardData is SellableCardData sellableCardData)
            {
                sellableCardDataHolder.Price = sellableCardData.Price.Value;
            }

            return sellableCardDataHolder;
        }

        private DamageableCardDataHolder CreateDamageableCardDataHolder(global::Cards.Data.CardData cardData)
        {
            DamageableCardDataHolder damageableCardDataHolder = new DamageableCardDataHolder(CreateBaseCardDataHolder(cardData));

            if (cardData is DamageableCardData damageableCardData)
            {
                damageableCardDataHolder.Health = damageableCardData.MaxHealth.Value;
            }

            return damageableCardDataHolder;
        }

        private FoodCardDataHolder CreateFoodCardDataHolder(global::Cards.Data.CardData cardData)
        {
            FoodCardDataHolder foodCardDataHolder = new FoodCardDataHolder(CreateSellableCardDataHolder(cardData));

            if (cardData is FoodCardData foodCardData)
            {
                foodCardDataHolder.NutritionalValue = foodCardData.MaxNutritionalValue.Value;
            }


            return foodCardDataHolder;
        }

        private CardDataHolder CreateBaseCardDataHolder(global::Cards.Data.CardData cardData)
        {
            CardDataHolder cardDataHolder = new CardDataHolder();

            cardDataHolder.Name = cardData.Name.Value;
            cardDataHolder.NameColor = cardData.NameColor.Value;

            if (cardData.TryGetComponentInChildren(out CardBackgroundTag cardBackgroundTag)
                && cardBackgroundTag.TryGetComponent(out Image backgroundImage))
            {
                cardDataHolder.BodyColor = backgroundImage.color;
            }

            if (cardData.TryGetComponentInChildren(out CardHeaderTag cardHeaderTag)
                && cardHeaderTag.TryGetComponent(out Image header))
            {
                cardDataHolder.HeaderColor = header.color;
            }

            cardDataHolder.StatsIconColor = Color.black;
            cardDataHolder.Icon = cardData.Icon.Value;
            cardDataHolder.Description = "Card Description";

            return cardDataHolder;
        }

        #endregion
    }
}
