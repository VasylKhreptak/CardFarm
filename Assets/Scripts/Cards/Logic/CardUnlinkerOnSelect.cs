﻿using System;
using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardUnlinkerOnSelect : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _isSelectedSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObservingIfSelected();
        }

        private void OnDisable()
        {
            StopObservingIfSelected();
        }

        #endregion

        private void StartObservingIfSelected()
        {
            StopObservingIfSelected();
            _isSelectedSubscription = _cardData.IsSelected.Where(x => x).Subscribe(_ => OnSelected());
        }

        private void StopObservingIfSelected()
        {
            _isSelectedSubscription?.Dispose();
        }

        private void OnSelected()
        {
            _cardData.UnlinkFromUpper();
        }
    }
}
