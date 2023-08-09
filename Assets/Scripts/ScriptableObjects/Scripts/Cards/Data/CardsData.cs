using System;
using System.Collections.Generic;
using Cards.Core;
using Data.Cards;
using Data.Cards.Core;
using NaughtyAttributes;
using UnityEditor;
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
                    }
                }
            }

            Debug.Log($"Found Cards {cards.Count}");
        }

        private void AddCardData(CardData cardData)
        {
            
        }

        #endregion
    }
}
