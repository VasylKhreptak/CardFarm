using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.Logic.ProgressHeightUpdaters.Core
{
    public class BaseCardProgressHeightUnlinker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _unlinkedWorldHeight;

        private Vector3 _initialLocalPosition;

        private IDisposable _unlinkSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public virtual void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
            _transform = GetComponent<Transform>();
        }

        private void Awake()
        {
            _initialLocalPosition = _transform.localPosition;
        }

        protected virtual void OnDisable()
        {
            StopUnlinking();
        }

        #endregion

        private void UnlinkStep()
        {
            Vector3 position = _transform.position;
            position.y = _cardData.BaseHeight;
            _transform.position = position;
        }

        protected void StartUnlinking()
        {
            StopUnlinking();

            _unlinkSubscription = Observable
                .EveryUpdate()
                .DoOnSubscribe(UnlinkStep)
                .Subscribe(_ => UnlinkStep());
        }

        protected void StopUnlinking()
        {
            _unlinkSubscription?.Dispose();
            _transform.localPosition = _initialLocalPosition;
        }
    }
}
