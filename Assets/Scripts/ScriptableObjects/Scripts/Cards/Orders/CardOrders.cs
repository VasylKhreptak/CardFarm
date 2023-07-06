using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using SRF;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Orders
{
    [CreateAssetMenu(fileName = "CardOrders", menuName = "ScriptableObjects/CardOrders")]
    public class CardOrders : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private List<Card> _possibleCards = new List<Card>();
        [SerializeField] private List<CardWeight> _rewards = new List<CardWeight>();
        [SerializeField] private int _minCardsCount = 1;
        [SerializeField] private int _maxCardsCount = 3;

        public List<Card> PossibleCards => _possibleCards.ToList();
        public List<CardWeight> Rewards => _rewards.ToList();
        public int MinCardsCount => _minCardsCount;
        public int MaxCardsCount => _maxCardsCount;

        public int GetRandomCardsCount() => Random.Range(_minCardsCount, _maxCardsCount + 1);

        public Card GetRandomCard() => _possibleCards.Random();
    }
}
