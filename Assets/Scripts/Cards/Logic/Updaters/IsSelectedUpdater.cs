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
        [SerializeField] private CardData _cardData;

        private IDisposable _mouseDownSubscription;
        private IDisposable _canBeSelectedSubscription;
        private IDisposable _mouseUpSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
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
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ =>
            {
                if (_cardData.CanBeSelected.Value)
                {
                    _cardData.IsSelected.Value = true;
                }
            });
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }

        private void StartObservingMouseUp()
        {
            StopObservingMouseUp();
            _mouseUpSubscription = _cardData.MouseTrigger.OnMouseUpAsObservable().Subscribe(_ =>
            {
                _cardData.IsSelected.Value = false;
            });
        }

        private void StopObservingMouseUp()
        {
            _mouseUpSubscription?.Dispose();
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
