using System;
using CameraManagement.CameraAim.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using CardsTable.ManualCardSelectors;
using Constraints.CardTable;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
using Zenject;

namespace Shop.ShopItems
{
    public class ShopItemCardSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ShopItemBuyEventInvoker _shopItemBuyEventInvoker;

        [Header("Preferences")]
        [SerializeField] private Card _targetCard;

        public event Action<Card> onSpawnedCard;

        private CardSpawner _cardSpawner;
        private ShopPanel _shopPanel;
        private CameraAimer _cameraAimer;
        private PlayingAreaTableBounds _playingAreaTableBounds;
        private InvestigatedCardsObserver _investigatedCardsObserver;
        private NewCardPanel _newCardPanel;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            ShopPanel shopPanel,
            CameraAimer cameraAimer,
            PlayingAreaTableBounds playingAreaTableBounds,
            InvestigatedCardsObserver investigatedCardsObserver,
            NewCardPanel newCardPanel)
        {
            _cardSpawner = cardSpawner;
            _shopPanel = shopPanel;
            _cameraAimer = cameraAimer;
            _playingAreaTableBounds = playingAreaTableBounds;
            _investigatedCardsObserver = investigatedCardsObserver;
            _newCardPanel = newCardPanel;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _shopItemBuyEventInvoker.onBought += SpawnCard;
        }

        private void OnDestroy()
        {
            _shopItemBuyEventInvoker.onBought -= SpawnCard;
        }

        #endregion

        private void SpawnCard()
        {
            bool canShowNewCardPanel = false;

            canShowNewCardPanel = _investigatedCardsObserver.IsInvestigated(_targetCard) == false;

            CardData spawnedCard = _cardSpawner.Spawn(_targetCard, _playingAreaTableBounds.transform.position);

            onSpawnedCard?.Invoke(spawnedCard.Card.Value);

            _shopPanel.Hide();
            _cameraAimer.Aim(spawnedCard.transform, true);

            if (canShowNewCardPanel)
            {
                _newCardPanel.Show(spawnedCard, 0.5f, onStart: () =>
                {
                    _cameraAimer.StopAiming();
                });
            }
        }
    }
}
