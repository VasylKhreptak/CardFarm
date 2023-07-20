using Cards.Zones.BuyZone.Data;
using Coins;
using Graphics.UI.Particles.Coins.Logic;
using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic
{
    public class BuyZoneCoinsCollector : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _cardData;

        private bool _canCollectCoins = true;

        private CoinsProvider _coinsProvider;
        private CoinsSpender _coinsSpender;
        private Camera _camera;

        [Inject]
        private void Constructor(CoinsSpender coinsSpender,
            CoinsProvider coinsProvider,
            CameraProvider cameraProvider)
        {
            _coinsSpender = coinsSpender;
            _coinsProvider = coinsProvider;
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<BuyZoneData>(true);
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
            _cardData.Callbacks.onClicked += OnClicked;
        }

        private void StopObservingClick()
        {
            _cardData.Callbacks.onClicked -= OnClicked;
        }

        private void OnClicked()
        {
            CollectCoins();
        }

        private void CollectCoins()
        {
            if (_canCollectCoins == false) return;

            int price = _cardData.Price.Value;

            int coinsCount = _coinsProvider.GetCoinsCount();

            coinsCount = Mathf.Min(price, _cardData.LeftCoins.Value);

            if (coinsCount == 0) return;

            int coinsToSpawn = Mathf.Min(price, coinsCount);

            Vector3 coinMoveDestination = RectTransformUtility.WorldToScreenPoint(_camera, _cardData.transform.position);

            _coinsSpender.Spend(coinsToSpawn, coinMoveDestination,
                () =>
                {
                    _cardData.CollectedCoins.Value += 1;
                },
                () =>
                {
                    _canCollectCoins = true;
                });

            _canCollectCoins = false;
        }
    }
}
