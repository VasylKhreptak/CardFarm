using System;
using Cards.Data;
using Physics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardFollowTouch : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CardData _cardData;

        [Header("PReferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _cardYOffset = 0.1f;
        [SerializeField] private float _speed = 15f;

        private IDisposable _mouseDownSubscription;
        private IDisposable _mouseUpSubscription;
        private IDisposable _followTouchSubscription;

        private ScreenRaycaster _screenRaycaster;

        [Inject]
        private void Constructor(ScreenRaycaster screenRaycaster)
        {
            _screenRaycaster = screenRaycaster;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
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
            StartObservingMouseDown();
            StartObservingMouseUp();
        }

        private void StopObserving()
        {
            StopObservingMouseDown();
            StopObservingMouseUp();
            StopFollowingTouch();
        }

        private void StartObservingMouseDown()
        {
            StopObservingMouseDown();
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ => StartFollowingTouch());
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }

        private void StartObservingMouseUp()
        {
            StopObservingMouseUp();
            _mouseUpSubscription = _cardData.MouseTrigger.OnMouseUpAsObservable().Subscribe(_ => StopFollowingTouch());
        }

        private void StopObservingMouseUp()
        {
            _mouseUpSubscription?.Dispose();
        }

        private void StartFollowingTouch()
        {
            StopFollowingTouch();
            _followTouchSubscription = Observable.EveryUpdate().Subscribe(_ => FollowTouchStep());
        }

        private void StopFollowingTouch()
        {
            _followTouchSubscription?.Dispose();
        }

        private void FollowTouchStep()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (_screenRaycaster.Raycast(mousePosition, _floorLayerMask, out RaycastHit hit))
            {
                Vector3 position = _transform.position;
                Vector3 targetCardPosition = hit.point + Vector3.up * _cardYOffset;
                _transform.position = Vector3.Lerp(position, targetCardPosition, _speed * Time.deltaTime);
            }
        }
    }
}
