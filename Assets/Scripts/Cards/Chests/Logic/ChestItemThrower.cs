using System;
using Cards.Chests.Data;
using Cards.Core;
using Cards.Logic.Spawn;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.Logic
{
    public class ChestItemThrower : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestData _chestData;

        [Header("Preferences")]
        [SerializeField] private int _minItemsToThrow = 1;
        [SerializeField] private int _maxItemsToThrow = 4;
        [SerializeField] private int _itemsToThrowIncrement = 1;
        [SerializeField] private int _incrementEach = 2;
        [SerializeField] private float _itemsToThrowResetCooldown = 1f;
        [SerializeField] private float _itemThrowInterval = 0.1f;

        private int _itemsToThrow;
        private int _clickedTimes;

        private IDisposable _resetCooldownSubscription;
        private CompositeDisposable _throwIntervalSubscriptions = new CompositeDisposable();

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<ChestData>(true);
        }

        private void OnEnable()
        {
            ResetValues();
            StartObservingClick();
        }

        private void OnDisable()
        {
            StopObservingClick();
            _resetCooldownSubscription?.Dispose();
            ResetValues();
            _throwIntervalSubscriptions.Clear();
        }

        #endregion

        private void StartObservingClick()
        {
            _chestData.Callbacks.onClicked += OnClick;
        }

        private void StopObservingClick()
        {
            _chestData.Callbacks.onClicked -= OnClick;
        }

        private void OnClick()
        {
            if (CanThrowItems() == false) return;

            TryIncreaseItemsToThrow();

            ThrowItems();
        }

        private void TryIncreaseItemsToThrow()
        {
            _clickedTimes++;

            if (_clickedTimes % _incrementEach == 0)
            {
                _itemsToThrow += _itemsToThrowIncrement;
                _itemsToThrow = Mathf.Min(_itemsToThrow, _maxItemsToThrow);
            }

            _resetCooldownSubscription?.Dispose();
            _resetCooldownSubscription = Observable
                .Timer(TimeSpan.FromSeconds(_itemsToThrowResetCooldown))
                .Subscribe(_ =>
                {
                    ResetValues();
                });
        }

        private void ResetValues()
        {
            _itemsToThrow = _minItemsToThrow;
            _clickedTimes = 0;
        }

        private void ThrowItems()
        {
            int itemsToThrow = Mathf.Min(_chestData.Size.Value, _itemsToThrow);

            Card cardToSpawn = _chestData.ChestType.Value.Value;

            float delay = 0f;

            for (int i = 0; i < itemsToThrow; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                {
                    _cardSpawner.SpawnAndMove(cardToSpawn, _chestData.transform.position);
                }).AddTo(_throwIntervalSubscriptions);

                _chestData.StoredCards.RemoveAt(0);

                delay += _itemThrowInterval;
            }
        }

        private bool CanThrowItems()
        {
            return _chestData.Size.Value > 0 && _chestData.ChestType.Value.HasValue;
        }

    }
}
