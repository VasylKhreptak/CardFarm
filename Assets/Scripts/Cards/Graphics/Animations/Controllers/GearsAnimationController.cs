﻿using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations.Controllers
{
    public class GearsAnimationController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _gearsObject;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _gearsObject = transform.GetChild(0).gameObject;
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
            StopObserving();

            _subscription = _cardData.CanBeUnderSelectedCard.Subscribe(canBeParent =>
            {
                _gearsObject.SetActive(canBeParent);
            });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }
    }
}