using System;
using Economy;
using Extensions;
using Graphics.UI.Particles.Coins.Logic;
using Providers.Graphics.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Zones.BuyZone.Data;

namespace Zones.BuyZone.Logic
{
    public class BuyZoneCoinsCollector : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _zoneData;

        private IDisposable _clickSubscription;

        private bool _canCollectCoins = true;

        private CoinsSpender _coinsSpender;
        private CoinsBank _coinsBank;
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        [Inject]
        private void Constructor(CoinsSpender coinsSpender,
            CoinsBank coinsBank,
            CanvasProvider canvasProvider)
        {
            _coinsSpender = coinsSpender;
            _coinsBank = coinsBank;
            _canvas = canvasProvider.Value;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _zoneData = GetComponentInParent<BuyZoneData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _canCollectCoins = true;
        }

        #endregion

        private void StartObserving()
        {
            StartObservingClick();
        }

        private void StopObserving()
        {
            StopObservingClick();
        }

        private void StartObservingClick()
        {
            StopObservingClick();
            _clickSubscription = _zoneData.BackgroundBehaviour.OnPointerClickAsObservable().Subscribe(_ => OnClicked());
        }

        private void StopObservingClick()
        {
            _clickSubscription?.Dispose();
        }

        private void OnClicked()
        {
            CollectCoins();
        }

        private void CollectCoins()
        {
            if (_canCollectCoins == false) return;

            int totalCoinsCount = _coinsBank.Value;

            int coinsToSpawn = Mathf.Min(_zoneData.LeftCoinsCount.Value, totalCoinsCount);

            if (coinsToSpawn == 0) return;
            
            Debug.Log($"Collecting {coinsToSpawn} coins");

            _coinsSpender.Spend(coinsToSpawn,
                () => ConvertPoint(_zoneData.transform.position),
                () =>
                {
                    _zoneData.CollectedCoinsCount.Value += 1;
                },
                () =>
                {
                    _canCollectCoins = true;
                });

            _canCollectCoins = false;
        }

        private Vector3 ConvertPoint(Vector3 point)
        {
            return RectTransformUtilityExtensions.ProjectPointOnCameraCanvas(_canvas, _canvasRectTransform, point);
        }
    }
}
