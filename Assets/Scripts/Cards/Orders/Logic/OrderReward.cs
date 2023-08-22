using System;
using Cards.Core;
using Cards.Logic.Spawn;
using Cards.Orders.Data;
using Extensions;
using Graphics.UI.Particles.Coins.Logic;
using Providers.Graphics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Orders.Logic
{
    public class OrderReward : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private OrderData _orderData;

        private IDisposable _isOrderCompletedSubscription;

        private CardSpawner _cardSpawner;
        private CoinsCollector _coinsCollector;
        private Camera _camera;
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        [Inject]
        private void Constructor(CardSpawner cardSpawner,
            CoinsCollector coinsCollector,
            CameraProvider cameraProvider,
            Canvas canvas)
        {
            _cardSpawner = cardSpawner;
            _coinsCollector = coinsCollector;
            _camera = cameraProvider.Value;
            _canvas = canvas;
            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _orderData = GetComponentInParent<OrderData>(true);
        }

        private void OnEnable()
        {
            StartObservingOrderCompletion();
        }

        private void OnDisable()
        {
            StopObservingOrderCompletion();
        }

        #endregion

        private void StartObservingOrderCompletion()
        {
            StopObservingOrderCompletion();
            _isOrderCompletedSubscription = _orderData.IsOrderCompleted.Subscribe(OnOrderCompletionStateUpdated);
        }

        private void StopObservingOrderCompletion()
        {
            _isOrderCompletedSubscription?.Dispose();
        }

        private void OnOrderCompletionStateUpdated(bool isCompleted)
        {
            if (isCompleted)
            {
                SpawnReward();
            }
        }

        private void SpawnReward()
        {
            Card cardToSpawn = _orderData.Rewards.GetByWeight(x => x.Weight).Card;

            if (cardToSpawn == Card.Coin)
            {
                Vector3 spawnPosition = RectTransformUtilityExtensions.ProjectPointOnCameraCanvas(_canvas, _canvasRectTransform, _orderData.transform.position);
                _coinsCollector.Collect(1, spawnPosition, 0f);
            }
            else
            {
                _cardSpawner.SpawnAndMove(cardToSpawn, _orderData.transform.position);
            }

            _orderData.IsOrderCompleted.Value = false;
            _orderData.gameObject.SetActive(false);
        }
    }
}
