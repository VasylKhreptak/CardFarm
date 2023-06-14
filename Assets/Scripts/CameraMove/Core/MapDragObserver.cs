using System;
using Providers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CameraMove.Core
{
    public class MapDragObserver : MonoBehaviour
    {
        private IDisposable _dragSubscription;
        private IDisposable _floorMouseDownSubscription;
        private IDisposable _floorMouseUpSubscription;

        private Vector2ReactiveProperty _delta = new Vector2ReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2> Delta => _delta;

        private SafeAreaProvider _safeAreaProvider;
        private ObservableMouseTrigger _floorMouseTrigger;

        [Inject]
        private void Constructor(SafeAreaProvider safeAreaProvider,
            FloorMouseTriggerProvider floorMouseTrigger)
        {
            _safeAreaProvider = safeAreaProvider;
            _floorMouseTrigger = floorMouseTrigger.Value;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingFloorMouseDown();
            StartObservingFloorMouseUp();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StopObserving()
        {
            StopObservingDrag();
            StopObservingFloorMouseDown();
            StopObservingFloorMouseUp();
        }

        private void StartObservingFloorMouseDown()
        {
            StopObservingFloorMouseDown();
            _floorMouseDownSubscription = _floorMouseTrigger
                .OnMouseDownAsObservable()
                .Subscribe(_ =>
                {
                    StartObservingDrag();
                });
        }

        private void StopObservingFloorMouseDown()
        {
            _floorMouseDownSubscription?.Dispose();
        }

        private void StartObservingFloorMouseUp()
        {
            StopObservingFloorMouseUp();
            _floorMouseUpSubscription = _floorMouseTrigger
                .OnMouseUpAsObservable()
                .Subscribe(_ =>
                {
                    StopObservingDrag();
                });
        }

        private void StopObservingFloorMouseUp()
        {
            _floorMouseUpSubscription?.Dispose();
        }

        private void StartObservingDrag()
        {
            StopObservingDrag();

            _dragSubscription = _safeAreaProvider.Value.Behaviour
                .OnDragAsObservable()
                .Subscribe(dragData =>
                {
                    _delta.Value = dragData.delta;
                });
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }
    }
}
