using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class UpperCardDeselector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _mouseDownSubscription;

        #region MonoBehaviour

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

            _mouseDownSubscription = _cardData.MouseTrigger.OnMouseDownAsObservable().Subscribe(_ =>
            {
                _cardData.UpperCard.Value = null;
            });
        }

        private void StopObservingMouseDown()
        {
            _mouseDownSubscription?.Dispose();
        }
    }
}
