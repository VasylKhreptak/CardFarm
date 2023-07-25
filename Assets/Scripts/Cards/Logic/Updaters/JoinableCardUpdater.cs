using System;
using System.Collections.Generic;
using Cards.Data;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class JoinableCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("Preferences")]
        [SerializeField] private Transform[] _cardCorners;
        [SerializeField] private CardData _cardData;
        [SerializeField] private LayerMask _cardsLayer;
        [SerializeField] private float _raycastDistance = 4f;
        [SerializeField] private float _cardUpdateInterval = 0.1f;
        [SerializeField] private int _hitsBufferSize = 20;

        private RaycastHit[] _hits;

        private IDisposable _isCardSelectedSubscription;
        private IDisposable _intervalSubscription;
        private IDisposable _frameDelaySubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void Awake()
        {
            _hits = new RaycastHit[_hitsBufferSize];
        }

        private void OnEnable()
        {
            StartObservingCardSelection();
        }

        private void OnDisable()
        {
            StopObservingCardSelection();
            StopFrameDelay();
            StopUpdatingJoinableCard();
            ResetJoinableCard();
        }

        #endregion

        private void StartObservingCardSelection()
        {
            StopObservingCardSelection();

            _isCardSelectedSubscription = _cardData.IsSelected.Subscribe(IsCardSelectedValueUpdated);
        }

        private void StopObservingCardSelection()
        {
            _isCardSelectedSubscription?.Dispose();
        }

        private void IsCardSelectedValueUpdated(bool isCardSelected)
        {
            if (isCardSelected)
            {
                ResetJoinableCard();
                StartUpdatingJoinableCard();
            }
            else
            {
                StopUpdatingJoinableCard();
                ResetJoinableCardFrameDelayed();
            }
        }

        private void ResetJoinableCardFrameDelayed()
        {
            StopFrameDelay();
            _frameDelaySubscription = Observable.NextFrame().Subscribe(_ =>
            {
                ResetJoinableCard();
            });
        }

        private void ResetJoinableCard()
        {
            _cardData.JoinableCard.Value = null;
        }

        private void StopFrameDelay()
        {
            _frameDelaySubscription?.Dispose();
        }

        private void StartUpdatingJoinableCard()
        {
            StopUpdatingJoinableCard();

            _intervalSubscription = Observable.Interval(TimeSpan.FromSeconds(_cardUpdateInterval))
                .Subscribe(_ =>
                {
                    UpdateJoinableCard();
                });
        }

        private void StopUpdatingJoinableCard()
        {
            _intervalSubscription?.Dispose();
        }

        private void UpdateJoinableCard()
        {
            _cardData.JoinableCard.Value = FindJoinableCard();
        }

        private CardData FindJoinableCard()
        {
            for (int i = 0; i < _cardCorners.Length; i++)
            {
                Transform cardCorner = _cardCorners[i];
                Vector3 rayOrigin = cardCorner.position + Vector3.up * _raycastDistance / 2f;
                Ray ray = new Ray(rayOrigin, Vector3.down);

                int hitsCount = UnityEngine.Physics.RaycastNonAlloc(ray, _hits, _raycastDistance, _cardsLayer);

                for (int j = 0; j < hitsCount; j++)
                {
                    RaycastHit hit = _hits[j];

                    if (hit.collider.TryGetComponent(out CardData card)
                        && card != _cardData
                        && card.CanBeUnderSelectedCard.Value)
                    {
                        return card;
                    }
                }
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            if (_cardCorners == null || _cardCorners.Length == 0) return;

            List<Vector3> rayOrigins = new List<Vector3>(_cardCorners.Length);

            foreach (Transform corner in _cardCorners)
            {
                rayOrigins.Add(corner.position + Vector3.up * _raycastDistance / 2f);
            }

            Gizmos.color = Color.red;
            foreach (Vector3 rayOrigin in rayOrigins)
            {
                Gizmos.DrawRay(rayOrigin, Vector3.down * _raycastDistance);
            }
        }
    }
}
