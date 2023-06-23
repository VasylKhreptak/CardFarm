using System;
using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;

namespace Cards
{
    public class MouseCardUnlinker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _mouseDownSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
        }
        
        private void OnEnable()
        {
            StartObservingMouseDown();
        }

        private void OnDisable()
        {
            StopObservingMouseDown();
        }

        #endregion

        private void StartObservingMouseDown()
        {
            StopObservingMouseDown();
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ => OnMouseDown());
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }

        private void OnMouseDown()
        {
            _cardData.UnlinkFromUpper();
        }
    }
}
