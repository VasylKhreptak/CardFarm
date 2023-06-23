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
            _cardData ??= GetComponentInParent<CardData>();
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

        private void OnMouseDownAsObservable()
        {
            
        }
        
        private void StartFollowingTouch()
        {
            StopFollowingTouch();

            Vector3 mousePosition = Input.mousePosition;
            if (_screenRaycaster.Raycast(mousePosition, _floorLayerMask, out RaycastHit hit))
            {
                Vector3 difference = _transform.position - hit.point;
                Vector2 offset = new Vector2(difference.x, difference.z);
                _followTouchSubscription = Observable.EveryUpdate().Subscribe(_ => FollowTouchStep(offset));
            }
        }

        private void StopFollowingTouch()
        {
            _followTouchSubscription?.Dispose();
        }

        private void FollowTouchStep(Vector2 horOffset)
        {
            if (Input.touchCount > 1)
            {
                StopFollowingTouch();
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            if (_screenRaycaster.Raycast(mousePosition, _floorLayerMask, out RaycastHit hit))
            {
                Vector3 cardPosition = _transform.position;
                Vector3 targetCardPosition = hit.point + new Vector3(horOffset.x, 0f, horOffset.y);
                cardPosition = Vector3.Lerp(cardPosition, targetCardPosition, _speed * Time.deltaTime);
                cardPosition.y = _cardData.Height.Value;
                _transform.position = cardPosition;
            }
        }
    }
}
