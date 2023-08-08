using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsSelectedUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _canBeSelectedSubscription;

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
            _cardData.IsSelected.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StartObservingIfCanBeSelected();
            StartObservingMouseDown();
            StartObservingMouseUp();
        }

        private void StopObserving()
        {
            StopObservingIfCanBeSelected();
            StopObservingMouseDown();
            StopObservingMouseUp();

            _cardData.IsSelected.Value = false;
        }

        private void StartObservingMouseDown()
        {
            StopObservingMouseDown();
            _cardData.Callbacks.onPointerDown += OnMouseDown;
        }

        private void StopObservingMouseDown()
        {
            _cardData.Callbacks.onPointerDown -= OnMouseDown;
        }

        private void StartObservingMouseUp()
        {
            StopObservingMouseUp();
            _cardData.Callbacks.onPointerUp += OnMouseUp;
        }

        private void StopObservingMouseUp()
        {
            _cardData.Callbacks.onPointerUp -= OnMouseUp;
        }

        private void OnMouseDown()
        {
            if (_cardData.CanBeSelected.Value)
            {
                _cardData.IsSelected.Value = true;
            }
        }

        private void OnMouseUp()
        {
            _cardData.IsSelected.Value = false;
        }

        private void StartObservingIfCanBeSelected()
        {
            StopObservingIfCanBeSelected();

            _canBeSelectedSubscription = _cardData.CanBeSelected.Where(x => x == false).Subscribe(canBeSelected =>
            {
                _cardData.IsSelected.Value = false;
            });
        }

        private void StopObservingIfCanBeSelected()
        {
            _canBeSelectedSubscription?.Dispose();
        }
    }
}
