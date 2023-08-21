using System;
using Cards.Data;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardHeightController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Smooth Height Change Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;

        private BoolReactiveProperty _isUpdatingHeightSmoothly = new BoolReactiveProperty(false);

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private IDisposable _upperCardHeightSubscription;
        private IDisposable _upperCardSubscription;

        private Tween _heightTween;

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
            StartObservingUpperCard();
        }

        private void OnDisable()
        {
            StopObserving();
            KillHeightTween();
            StopObservingUpperCard();
            _upperCardHeightSubscription?.Dispose();
            ResetHeight();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.IsSelected.Subscribe(_ => OnCardDataChanged()).AddTo(_subscriptions);
            _cardData.JoinableCard.Subscribe(_ => OnCardDataChanged()).AddTo(_subscriptions);
            _cardData.UpperCard.Subscribe(_ => OnCardDataChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnCardDataChanged()
        {
            bool isSelected = _cardData.IsSelected.Value;
            CardData joinableCard = _cardData.JoinableCard.Value;
            CardData upperCard = _cardData.UpperCard.Value;

            KillHeightTween();

            if (isSelected)
            {
                SetHeightSmooth(_cardData.SelectedHeight);

                return;
            }

            if (upperCard != null)
            {
                SetHeightSmooth(upperCard.transform);
            }
            else if (joinableCard != null)
            {
                SetHeightSmooth(joinableCard.transform);
            }
            else
            {
                SetHeightSmooth(_cardData.BaseHeight);
            }
        }

        private float GetHeight() => _cardData.transform.position.y;

        private void SetHeight(float height)
        {
            Vector3 position = _cardData.transform.position;
            position.y = height;
            _cardData.Height.Value = height;
            _cardData.transform.position = position;
        }

        private void SetHeightSmooth(float height)
        {
            KillHeightTween();

            _heightTween = DOTween
                .To(GetHeight, SetHeight, height, _duration)
                .SetEase(_curve)
                .OnPlay(() => _isUpdatingHeightSmoothly.Value = true)
                .OnKill(() => _isUpdatingHeightSmoothly.Value = false)
                .OnComplete(() => _isUpdatingHeightSmoothly.Value = false)
                .Play();
        }

        private void SetHeightSmooth(Transform target)
        {
            KillHeightTween();

            float progress = 0;
            float startHeight = GetHeight();
            _heightTween = DOTween.To(() => progress, x => progress = x, 1, _duration)
                .SetEase(_curve)
                .OnUpdate(() => SetHeight(Mathf.Lerp(startHeight, target.position.y, progress)))
                .OnPlay(() => _isUpdatingHeightSmoothly.Value = true)
                .OnKill(() => _isUpdatingHeightSmoothly.Value = false)
                .OnComplete(() => _isUpdatingHeightSmoothly.Value = false)
                .Play();
        }

        private void KillHeightTween()
        {
            _heightTween?.Kill();
        }

        private void StartObservingUpperCard()
        {
            StopObservingUpperCard();
            _upperCardHeightSubscription?.Dispose();

            _upperCardSubscription?.Dispose();
            _upperCardSubscription = _cardData.UpperCard.Subscribe(upperCard =>
            {
                _upperCardHeightSubscription?.Dispose();

                if (upperCard == null) return;

                _upperCardHeightSubscription = upperCard.Height.Subscribe(height =>
                {
                    if (_isUpdatingHeightSmoothly.Value
                        || _cardData.IsSelected.Value
                        || _cardData.JoinableCard.Value != null) return;

                    SetHeight(height);
                });
            });
        }

        private void StopObservingUpperCard()
        {
            _upperCardHeightSubscription?.Dispose();
            _upperCardSubscription?.Dispose();
        }

        public void ResetHeight()
        {
            SetHeight(_cardData.BaseHeight);
        }
    }
}
