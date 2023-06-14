﻿using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic
{
    public class CardFollowCard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _speed = 5f;

        private IDisposable _topCardSubscription;
        private IDisposable _updateDisposable;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnEnable()
        {
            StartObservingTopCard();
        }

        private void OnDisable()
        {
            StopObservingTopCard();
            StopFollowing();
        }

        #endregion

        private void StartObservingTopCard()
        {
            StopObservingTopCard();
            _topCardSubscription = _cardData.UpperCard.Subscribe(topCard =>
            {
                if (topCard == null)
                {
                    StopFollowing();
                    Debug.Log("Stop");
                }
                else
                {
                    StartFollowing();
                    Debug.Log("Start");
                }
            });
        }

        private void StopObservingTopCard()
        {
            _topCardSubscription?.Dispose();
        }

        private void StartFollowing()
        {
            StopFollowing();
            _updateDisposable = Observable.EveryUpdate().Subscribe(_ => FollowStep());
        }

        private void StopFollowing()
        {
            _updateDisposable?.Dispose();
        }

        private void FollowStep()
        {
            Vector3 targetPosition = _cardData.UpperCard.Value.BottomCardFollowPoint.position;
            Vector3 transformPosition = _transform.position;

            transformPosition = Vector3.Lerp(transformPosition, targetPosition, _speed * Time.deltaTime);
            transformPosition.y = _cardData.Height.Value;
            _transform.position = transformPosition;
        }
    }
}
