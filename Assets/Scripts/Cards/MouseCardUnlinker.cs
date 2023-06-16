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

        private void OnEnable()
        {
            _mouseDownSubscription?.Dispose();
            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ =>
            {
                _cardData.UnlinkFromUpper();
            });
        }

        private void OnDisable()
        {
            _mouseDownSubscription?.Dispose();
        }

        #endregion
    }
}
