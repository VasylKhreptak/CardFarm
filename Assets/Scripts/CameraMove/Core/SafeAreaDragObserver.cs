using Providers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace CameraMove.Core
{
    public class SafeAreaDragObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _smoothSpeed = 10f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private Vector2ReactiveProperty _delta = new Vector2ReactiveProperty();
        private Vector2ReactiveProperty _smoothedDelta = new Vector2ReactiveProperty();
        private BoolReactiveProperty _isDragging = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2> Delta => _delta;
        public IReadOnlyReactiveProperty<Vector2> SmoothedDelta => _smoothedDelta;
        public IReadOnlyReactiveProperty<bool> IsDragging => _isDragging;

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

        private void Update()
        {
            _smoothedDelta.Value = Vector2.Lerp(_smoothedDelta.Value, _delta.Value, _smoothSpeed * Time.deltaTime);
        }

        #endregion

        private void StartObservingDrag()
        {
            StopObservingDrag();

            _safeAreaProvider.Value.Behaviour
                .OnBeginDragAsObservable()
                .Merge(_safeAreaProvider.Value.Behaviour.OnEndDragAsObservable())
                .Subscribe(dragData =>
                {
                    if (_safeAreaProvider.Value.TouchCounter.TouchCount.Value != 1) return;

                    _delta.Value = Vector2.zero;
                    _isDragging.Value = true;
                }).AddTo(_subscriptions);

            _safeAreaProvider.Value.Behaviour.OnDragAsObservable().Subscribe(dragData =>
            {
                if (_safeAreaProvider.Value.TouchCounter.TouchCount.Value != 1) return;

                _delta.Value = dragData.delta * Time.deltaTime;
            }).AddTo(_subscriptions);
        }

        private void StopObservingDrag()
        {
            _subscriptions?.Clear();
        }
    }
}
