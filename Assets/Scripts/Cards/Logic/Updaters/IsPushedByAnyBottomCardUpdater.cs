using System;
using System.Collections.Generic;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsPushedByAnyBottomCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _updateInterval = 1 / 20f;
        [SerializeField] private float _threshold = 0.03f;

        private CompositeDisposable _subscriptions = new CompositeDisposable();
        private IDisposable _updateSubscription;

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
            StopUpdating();
            _cardData.IsPushedByAnyBottomCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.IsTopCard.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
            _cardData.IsPushable.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
            _cardData.IsPlayingAnyAnimation.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
            _cardData.IsOverlayed.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentUpdated()
        {
            if (_cardData.IsTopCard.Value
                && _cardData.IsAnyGroupCardSelected.Value == false
                && _cardData.IsPushable.Value
                && _cardData.IsPlayingAnyAnimation.Value == false
                && _cardData.IsOverlayed.Value == false)
            {
                StartUpdating();
            }
            else
            {
                StopUpdating();
                _cardData.IsPushedByAnyBottomCard.Value = false;
            }
        }

        private void StartUpdating()
        {
            StopUpdating();

            _updateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(OnIntervalTick)
                .Subscribe(_ => OnIntervalTick());
        }

        private void StopUpdating()
        {
            _updateSubscription?.Dispose();
        }

        private void OnIntervalTick()
        {
            _cardData.IsPushedByAnyBottomCard.Value = IsPushedByBottomCard();
        }

        private bool IsPushedByBottomCard()
        {
            List<CardData> bottomCards = _cardData.BottomCards;

            foreach (var bottomCard in bottomCards)
            {
                CardData upperCard = bottomCard.UpperCard.Value;

                if (upperCard == null) continue;

                Transform followPoint = upperCard.BottomCardFollowPoint;

                if (bottomCard.transform.position.z > followPoint.position.z + _threshold)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
