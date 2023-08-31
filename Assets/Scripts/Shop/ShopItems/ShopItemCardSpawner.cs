using CameraManagement.CameraAim.Core;
using Cards.Core;
using Cards.Data;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using UnityEngine;
using Zenject;

namespace Shop.ShopItems
{
    public class ShopItemCardSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ShopItemBuyEventInvoker _shopItemBuyEventInvoker;

        [Header("Preferences")]
        [SerializeField] private Card _targetCard;

        private CardSpawner _cardSpawner;
        private ShopPanel _shopPanel;
        private CameraAimer _cameraAimer;
        private PlayingAreaTableBounds _playingAreaTableBounds;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            ShopPanel shopPanel,
            CameraAimer cameraAimer,
            PlayingAreaTableBounds playingAreaTableBounds)
        {
            _cardSpawner = cardSpawner;
            _shopPanel = shopPanel;
            _cameraAimer = cameraAimer;
            _playingAreaTableBounds = playingAreaTableBounds;
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
            CardData spawnedCard = _cardSpawner.Spawn(_targetCard, _playingAreaTableBounds.transform.position);

            _shopPanel.Hide();
            _cameraAimer.Aim(spawnedCard.transform, true);
        }
    }
}
