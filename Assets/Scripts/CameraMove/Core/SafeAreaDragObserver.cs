using System;
using Providers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CameraMove.Core
{
    public class SafeAreaDragObserver : MonoBehaviour
    {
        private IDisposable _dragSubscription;

        private Vector2ReactiveProperty _delta = new Vector2ReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2> Delta => _delta;

        private SafeAreaProvider _safeAreaProvider;

        [Inject]
        private void Constructor(SafeAreaProvider safeAreaProvider)
        {
            _safeAreaProvider = safeAreaProvider;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingDrag();
        }

        private void OnDisable()
        {
            StopObservingDrag();
        }
        
        #endregion

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
