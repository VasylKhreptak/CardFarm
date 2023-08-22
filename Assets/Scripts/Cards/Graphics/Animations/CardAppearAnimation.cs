using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using Constraints.CardTable;
using DG.Tweening;
using Extensions;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Graphics.Animations
{
    public class CardAppearAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale = Vector3.one;
        [SerializeField] private float _scaleDuration = 0.5f;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Delay Preferences")]
        [SerializeField] private float _delayBeforeJump = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpDuration = 0.5f;
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha = 1f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Random Jump Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        [Header("Flip Preferences")]
        [SerializeField] private float _flipDuration = 0.3f;
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private AnimationCurve _flipCurve;

        [Header("Raise Preferences")]
        [SerializeField] private float _raiseDelay = 0.5f;
        [SerializeField] private float _raiseDuration = 0.7f;
        [SerializeField] private float _raiseHeight = 0.6f;
        [SerializeField] private Vector3 _raiseScale;
        [SerializeField] private AnimationCurve _raiseHeightCurve;
        [SerializeField] private AnimationCurve _raiseScaleCurve;

        [Header("Place Preferences")]
        [SerializeField] private float _placeDuration;
        [SerializeField] private AnimationCurve _placeHeightCurve;
        [SerializeField] private AnimationCurve _placeScaleCurve;

        public event Action onRaised;

        private Sequence _sequence;

        private PlayingAreaTableBounds _bounds;

        [Inject]
        private void Constructor(PlayingAreaTableBounds bounds)
        {
            _bounds = bounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnDisable()
        {
            Stop();
            SetAlpha(_endAlpha);
            SetScale(_endScale);
            SetLocalRotation(_endLocalRotation);
        }

        #endregion

        public void Play(Vector3 targetPosition, Action onComplete = null)
        {
            Play(targetPosition, _jumpDuration, onComplete);
        }

        public void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            targetPosition = ClampPosition(targetPosition);

            _cardData.UnlinkFromUpper();

            Stop();
            SetScale(_startScale);
            SetAlpha(_startAlpha);
            SetLocalRotation(_startLocalRotation);

            _sequence = DOTween.Sequence();

            _sequence
                .OnPlay(() =>
                {
                    _isPlaying.Value = true;
                })
                .Join(_cardData.transform.DOScale(_endScale, _scaleDuration).SetEase(_scaleCurve))
                .Join(_cardData.CanvasGroup.DOFade(_endAlpha, _fadeDuration).SetEase(_fadeCurve))
                .AppendInterval(_delayBeforeJump)
                .Append(_cardData.transform.DOJump(targetPosition, _jumpPower, _numberOfJumps, duration).SetEase(_jumpCurve))
                .AppendInterval(_raiseDelay)
                .Append(_cardData.transform.DOMoveY(_raiseHeight, _raiseDuration).SetEase(_raiseHeightCurve))
                .Join(_cardData.transform.DOScale(_raiseScale, _raiseDuration).SetEase(_raiseScaleCurve))
                .AppendCallback(() => onRaised?.Invoke())
                .OnUpdate(() =>
                {
                    Vector3 position = _cardData.transform.position;
                    _cardData.Height.Value = position.y;
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                })
                .OnComplete(() =>
                {
                    _isPlaying.Value = false;
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _isPlaying.Value = false;
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                })
                .Play();
        }

        public void PlayRandomly(float minRange, float maxRange, Action onComplete = null)
        {
            PlayRandomly(minRange, maxRange, _jumpDuration, onComplete);
        }

        [Button()]
        public void PlayRandomly(Action onComplete = null)
        {
            PlayRandomly(_minRange, _maxRange, _jumpDuration, onComplete);
        }

        public void PlayRandomly(float minRange, float maxRange, float duration, Action onComplete = null)
        {
            Vector3 randomDirection = Random.insideUnitCircle;
            randomDirection.y = 0;
            float range = Random.Range(minRange, maxRange);
            Vector3 targetPosition = _cardData.transform.position + randomDirection * range;
            Play(targetPosition, duration, onComplete);
        }

        public override void Stop()
        {
            _sequence?.Kill();
            _isPlaying.Value = false;
        }

        private void SetScale(Vector3 scale) => _cardData.transform.localScale = scale;

        private void SetAlpha(float alpha) => _cardData.CanvasGroup.alpha = alpha;

        private void SetLocalRotation(Vector3 rotation) => _cardData.transform.localEulerAngles = rotation;

        private Vector3 ClampPosition(Vector3 position)
        {
            Vector3 clampedPosition = _bounds.Clamp(_cardData.RectTransform, position);
            clampedPosition.y = _cardData.BaseHeight;
            return clampedPosition;
        }

        public void PlaceCardOnTable(Action onComplete = null)
        {
            Stop();
            _sequence = DOTween.Sequence();

            _sequence
                .Append(_cardData.transform.DOLocalRotate(Vector3.zero, _flipDuration).SetEase(_flipCurve))
                .Append(_cardData.transform.DOMoveY(_cardData.BaseHeight, _placeDuration).SetEase(_placeHeightCurve))
                .Join(_cardData.transform.DOScale(Vector3.one, _placeDuration).SetEase(_placeScaleCurve))
                .OnUpdate(() =>
                {
                    _cardData.Height.Value = _cardData.transform.position.y;
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                })
                .OnComplete(() =>
                {
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _cardData.CardShirtStateUpdater.UpdateShirtState();
                })
                .Play();
        }
    }
}
