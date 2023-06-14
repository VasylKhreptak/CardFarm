﻿using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class CardHeightController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CardData _cardData;

        private IDisposable _upperCardHeightSubscription;

        private IDisposable _upperCardSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingUpperCard();
        }

        private void OnDisable()
        {
            StopObservingUpperCard();
        }

        #endregion

        private void StartObservingUpperCard()
        {
            StopObservingUpperCard();
            _upperCardSubscription = _cardData.UpperCard.Subscribe(UpdateHeight);
        }

        private void StopObservingUpperCard()
        {
            _upperCardSubscription?.Dispose();
            _upperCardHeightSubscription?.Dispose();
        }

        private void UpdateHeight(CardData upperCardData)
        {
            Vector3 position = _transform.position;

            _upperCardHeightSubscription?.Dispose();

            if (upperCardData == null)
            {
                position.y = _cardData.BaseHeight;
                _cardData.Height.Value = position.y;
            }
            else
            {
                _upperCardHeightSubscription = upperCardData.Height.Subscribe(height =>
                {
                    position.y = height + _cardData.HeightOffset;
                    _cardData.Height.Value = position.y;
                });
            }
        }
    }
}
