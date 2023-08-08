﻿using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CanBeSelectedUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.CanBeSelected.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            _cardData.IsInteractable.Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
            //_cardData.IsPlayingAnyAnimation.Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnStateUpdated()
        {
            bool canBeSelected =
                _cardData.IsInteractable.Value;
            //&& _cardData.IsPlayingAnyAnimation.Value == false;

            _cardData.CanBeSelected.Value = canBeSelected;
        }
    }
}
