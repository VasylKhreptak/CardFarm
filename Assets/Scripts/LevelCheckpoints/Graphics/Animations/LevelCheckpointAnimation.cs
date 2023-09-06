using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelCheckpoints.Graphics.Animations
{
    public class LevelCheckpointAnimation : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private LevelCheckpoint _levelCheckpoint;
        [SerializeField] private RectTransform _starIconRectTransform;
        [SerializeField] private Image _startIconImage;
        [SerializeField] private CanvasGroup _cardCanvasGroup;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private AnimationCurve _curve;

        [Header("Star Move Preferences")]
        [SerializeField] private Vector2 _startStarAnchoredPosition;
        [SerializeField] private Vector2 _endStarAnchoredPosition;

        [Header("Star Color Preferences")]
        [SerializeField] private Color _startStarColor;
        [SerializeField] private Color _endStarColor;

        private IDisposable _subscription;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _levelCheckpoint = GetComponentInParent<LevelCheckpoint>(true);
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

            _subscription = _levelCheckpoint.Reached.Subscribe(SetState);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void SetState(bool reachedState)
        {
            Stop();

            Tween cardTween = _cardCanvasGroup.DOFade(reachedState ? 0f : 1f, _duration).SetEase(_curve);
            Tween starMoveTween = _starIconRectTransform.DOAnchorPos(reachedState ? _endStarAnchoredPosition : _startStarAnchoredPosition, _duration).SetEase(_curve);
            Tween starColorTween = _startIconImage.DOColor(reachedState ? _endStarColor : _startStarColor, _duration).SetEase(_curve);

            _sequence = DOTween.Sequence();

            _sequence
                .Append(cardTween)
                .Join(starMoveTween)
                .Join(starColorTween)
                .Play();
        }

        private void Stop()
        {
            _sequence?.Kill();
        }
    }
}
