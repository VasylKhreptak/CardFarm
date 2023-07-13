using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Screen
{
    public class ScreenEdges : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _uiBehaviour;

        private BoolReactiveProperty _isPointerDown = new BoolReactiveProperty(false);
        private Vector2ReactiveProperty _directionFromCenter = new Vector2ReactiveProperty(Vector2.zero);

        private IDisposable _pointerDownSubscription;
        private IDisposable _pointerUpSubscription;

        public IReadOnlyReactiveProperty<bool> IsPointerDown => _isPointerDown;
        public IReadOnlyReactiveProperty<Vector2> DirectionFromCenter => _directionFromCenter;

        #region MonoBehaviour

        private void OnValidate()
        {
            _uiBehaviour ??= GetComponent<UIBehaviour>();
        }

        private void OnEnable()
        {
            StartObserving();
            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StartObservingPointerDown();
            StartObservingPointerUp();
        }

        private void StopObserving()
        {
            StopObservingPointerDown();
            StopObservingPointerUp();
        }

        private void StartObservingPointerDown()
        {
            StopObservingPointerDown();
            _pointerDownSubscription = _uiBehaviour.OnPointerDownAsObservable().Subscribe(_ => OnPointerDown());
        }

        private void StopObservingPointerDown()
        {
            _pointerDownSubscription?.Dispose();
        }

        private void StartObservingPointerUp()
        {
            StopObservingPointerUp();
            _pointerUpSubscription = _uiBehaviour.OnPointerUpAsObservable().Subscribe(_ => OnPointerUp());
        }

        private void StopObservingPointerUp()
        {
            _pointerUpSubscription?.Dispose();
        }

        private void OnPointerDown()
        {
            _isPointerDown.Value = true;
            Debug.Log("OnPointerDown");
        }

        private void OnPointerUp()
        {
            _isPointerDown.Value = false;
            Debug.Log("OnPointerUp");
        }
    }
}
