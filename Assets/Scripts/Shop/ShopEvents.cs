using System;
using Cards.Core;
using Shop.ShopItems;
using UnityEngine;

namespace Shop
{
    public class ShopEvents : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ShopItemCardSpawner[] _shopItemCardSpawners;

        public event Action<Card> onSpawnedCard;

        #region MonoBehaviour

        private void OnValidate()
        {
            _shopItemCardSpawners = GetComponentsInChildren<ShopItemCardSpawner>(true);
        }

        private void Awake()
        {
            foreach (var cardSpawner in _shopItemCardSpawners)
            {
                cardSpawner.onSpawnedCard += onSpawnedCard;
            }
        }

        private void OnDestroy()
        {
            foreach (var cardSpawner in _shopItemCardSpawners)
            {
                cardSpawner.onSpawnedCard -= onSpawnedCard;
            }
        }

        #endregion

        private void OnSpawnedCard(Card card)
        {
            onSpawnedCard?.Invoke(card);
        }
    }
}
