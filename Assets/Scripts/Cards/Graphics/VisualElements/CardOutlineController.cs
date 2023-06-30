﻿using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.VisualElements
{
    public class CardOutlineController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _outlineObject;

        private IDisposable _subscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _outlineObject ??= transform.GetChild(0).gameObject;
            _cardData ??= GetComponentInParent<CardData>(true);
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
            StopObserving();

            _subscription = _cardData.CanBeUnderSelectedCard.Subscribe(canBeParent =>
            {
                _outlineObject.SetActive(canBeParent);
            });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }
    }
}
