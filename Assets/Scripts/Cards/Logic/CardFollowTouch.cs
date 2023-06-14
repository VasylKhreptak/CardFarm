using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class CardFollowTouch : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CardData _cardData;

        [Header("PReferences")]
        [SerializeField] private float _speed = 15f;

        private IDisposable _mouseDownSubscription;
        private IDisposable _mouseUpSubscription;
        private IDisposable _followTouchSubscription;

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
            Vector3 position = _transform.position;
            Vector3 mousePosition = position;

            _transform.position = Vector3.Lerp(position, mousePosition, _speed * Time.deltaTime);
        }
    }
}
