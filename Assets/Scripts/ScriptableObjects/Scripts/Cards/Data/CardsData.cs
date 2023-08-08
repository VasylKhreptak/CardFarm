using System.Collections.Generic;
using Data.Cards;
using Data.Cards.Core;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Data
{
    [CreateAssetMenu(fileName = "CardsData", menuName = "ScriptableObjects/CardsData", order = 1)]
    public class CardsData : ScriptableObject
    {
        [SerializeReference] private List<CardDataHolder> _cardsData = new List<CardDataHolder>();

        [Button()]
        private void AddCardData()
        {
            _cardsData.Add(new CardDataHolder());
        }
        
        [Button()]
        private void AddSellableCardData()
        {
            _cardsData.Add(new SellableCardDataHolder());
        }
        
        [Button()]
        private void AddDamageableCardData()
        {
            _cardsData.Add(new DamageableCardDataHolder());
        }
        
        [Button()]
        private void AddFoodCardData()
        {
            _cardsData.Add(new FoodCardDataHolder());
        }
        
        [Button()]
        private void AddBoosterCardData()
        {
            _cardsData.Add(new BoosterCardDataHolder());
        }

        [Button()]
        private void ExecuteAction()
        {
            Debug.Log("Action");
        }
    }
}
