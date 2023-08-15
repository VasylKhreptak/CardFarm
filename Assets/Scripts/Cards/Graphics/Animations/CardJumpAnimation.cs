using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using Constraints.CardTable;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Graphics.Animations
{
    public class CardJumpAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Random Jump Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private Tween _animation;

        public float Duration => _duration;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
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

        protected virtual void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Vector3 targetPosition, Action onComplete = null)
        {
            Play(targetPosition, _duration, onComplete);
        }

        public virtual void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            targetPosition = ValidatePosition(targetPosition);

            _cardData.UnlinkFromUpper();

            Stop();

            _animation = _cardData.transform
                .DOJump(targetPosition, _jumpPower, _numberOfJumps, duration)
                .SetEase(_jumpCurve)
                .OnStart(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnUpdate(() =>
                {
                    Vector3 position = _cardData.transform.position;
                    float height = position.y;
                    _cardData.Height.Value = height;
                })
                .OnComplete(() =>
                {
                    _isPlaying.Value = false;
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _isPlaying.Value = false;
                })
                .Play();
        }

        public void PlayRandomly(float minRange, float maxRange, Action onComplete = null)
        {
            PlayRandomly(minRange, maxRange, _duration, onComplete);
        }

        public void PlayRandomly(Action onComplete = null)
        {
            PlayRandomly(_minRange, _maxRange, _duration, onComplete);
        }

        public void PlayRandomly(float minRange, float maxRange, float duration, Action onComplete = null)
        {
            Vector3 randomDirection = Random.insideUnitCircle;
            randomDirection.y = 0;
            float range = Random.Range(minRange, maxRange);
            Vector3 targetPosition = _cardData.transform.position + randomDirection * range;
            targetPosition = _cardsTableBounds.Clamp(_cardData.RectTransform, targetPosition);
            Play(targetPosition, duration, onComplete);
        }

        public override void Stop()
        {
            _animation?.Kill();
            _isPlaying.Value = false;
        }

        private Vector3 ValidatePosition(Vector3 position)
        {
            Vector3 clampedPosition = _cardsTableBounds.Clamp(_cardData.RectTransform, position);
            clampedPosition.y = _cardData.transform.position.y;
            return clampedPosition;
        }
    }
}
