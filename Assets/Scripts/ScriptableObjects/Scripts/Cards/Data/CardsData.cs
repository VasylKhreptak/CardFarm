using System.Collections.Generic;
using Cards.Core;
using Data.Cards;
using Data.Cards.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScriptableObjects.Scripts.Cards.Data
{
    [CreateAssetMenu(fileName = "CardsData", menuName = "ScriptableObjects/CardsData")]
    public class CardsData : ScriptableObject
    {
        [Header("Default Preferences")]
        [SerializeField] private CardDataHolder _defaultCardData;
        [SerializeField] private SellableCardDataHolder _defaultSellableCardData;
        [SerializeField] private DamageableCardDataHolder _defaultDamageableCardData;
        [SerializeField] private FoodCardDataHolder _defaultFoodCardData;
        [SerializeField] private BoosterCardDataHolder _defaultBoosterCardData;

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

        [Button()] private void AddBoosterCardData()
        {
            _cardsData.Add(new CardData
            {
                Key = default,
                Value = new BoosterCardDataHolder()
            });
        }

        [Button()]
        private void ExecuteAction()
        {
            Debug.Log("Action Executed");
        }

        [Button()]
        private void SyncWithPrefabs()
        {
            _cardsData.Clear();
        }
    }
}
