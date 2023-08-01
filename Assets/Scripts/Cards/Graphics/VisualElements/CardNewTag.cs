﻿using System;
using Cards.Data;
using Graphics.UI.Panels;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class CardNewTag : Panel, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _isNewCardSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
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
            _isNewCardSubscription = _cardData.IsNew.Subscribe(isNewCard =>
            {
                if (isNewCard)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            });
        }

        private void StopObserving()
        {
            _isNewCardSubscription?.Dispose();
        }
    }
}
