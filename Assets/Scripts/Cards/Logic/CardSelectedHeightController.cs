using System;
using Cards.Data;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardSelectedHeightController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _moveDuration = 0.3f;
        [SerializeField] private Ease _moveEase = Ease.OutCubic;

        private BoolReactiveProperty IsUpdatingHeight = new BoolReactiveProperty();

        private Tween _moveTween;

        private IDisposable _subscription;

        public IReadOnlyReactiveProperty<bool> IsUpdatingHeightProperty => IsUpdatingHeight;

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
            StopTween();
        }

        #endregion

        private void StartObserving()
        {
            _subscription = _cardData.IsSelected.Subscribe(UpdateHeight);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateHeight(bool isSelected)
        {
            if (isSelected)
            {
                SetCardHeight(_cardData.SelectedHeight);
            }
            else if (_cardData.JoinableCard.Value != null)
            {
                SetCardHeight(_cardData.JoinableCard.Value.Height.Value + _cardData.HeightOffset);
            }
            else
            {
                SetCardHeight(_cardData.BaseHeight);
            }
        }

        private void SetCardHeight(float height)
        {
            StopTween();

            _moveTween = _cardData.transform
                .DOMoveY(height, _moveDuration)
                .OnStart(() => IsUpdatingHeight.Value = true)
                .OnUpdate(() =>
                {
                    _cardData.Height.Value = _cardData.transform.position.y;
                })
                .SetEase(_moveEase)
                .OnKill(() => IsUpdatingHeight.Value = false)
                .Play();
        }

        private void StopTween()
        {
            _moveTween?.Kill();
        }
    }
}
