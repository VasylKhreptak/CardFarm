using System.Collections.Generic;
using Cards.Data;
using Cards.Logic.Spawn;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics
{
    public class CardLayerOrderUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private CompositeDisposable _delaySubscriptions = new CompositeDisposable();

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.Callbacks.onBecameHeadOfGroup += OnBecameHeadOfGroup;
            _cardData.IsSelected.Subscribe(IsCardSelectedValueChanged).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= OnBecameHeadOfGroup;
            _subscriptions.Clear();
            _cardSpawner.OnCardSpawnedNonParameterized -= RenderGroupOnTopFrameDelayed;
            _delaySubscriptions.Clear();
        }

        private void OnBecameHeadOfGroup()
        {
            RenderGroupOnTop();
        }

        private void IsCardSelectedValueChanged(bool isSelected)
        {
            if (isSelected)
            {
                RenderGroupOnTop();

                _cardSpawner.OnCardSpawnedNonParameterized -= RenderGroupOnTopFrameDelayed;
                _cardSpawner.OnCardSpawnedNonParameterized += RenderGroupOnTopFrameDelayed;
            }
            else
            {
                _cardSpawner.OnCardSpawnedNonParameterized -= RenderGroupOnTopFrameDelayed;
            }
        }

        private void RenderGroupOnTop()
        {
            RenderOnTop(_cardData.GroupCards);
        }

        private void RenderOnTop(List<CardDataHolder> cards)
        {
            foreach (var card in cards)
            {
                card.RenderOnTop();
            }
        }

        private void RenderGroupOnTopFrameDelayed()
        {
            Observable.NextFrame().Subscribe(_ =>
            {
                RenderGroupOnTop();
            }).AddTo(_delaySubscriptions);
        }
    }
}
