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
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private Ease _jumpEase;
        
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

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Vector3 targetPosition, Action onComplete = null)
        {
            Play(targetPosition, _duration, onComplete);
        }

        public void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            targetPosition = ValidatePosition(targetPosition);

            _cardData.UnlinkFromUpper();

            Stop();

            _animation = _cardData.transform
                .DOJump(targetPosition, _jumpPower, _numberOfJumps, duration)
                .SetEase(_jumpEase)
                .OnStart(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _isPlaying.Value = false;
                })
                .Play();
        }

        public void PlayRandomly(float minRange, float maxRange, Action onComplete = null)
        {
            PlayRandomly(minRange, maxRange, _duration, onComplete);
        }

        public void PlayRandomly(float minRange, float maxRange, float duration, Action onComplete = null)
        {
            Vector3 randomDirection = Random.insideUnitCircle;
            randomDirection.y = 0;
            float range = Random.Range(minRange, maxRange);
            Vector3 targetPosition = _cardData.transform.position + randomDirection * range;
            Bounds bounds = _cardData.Collider.bounds;
            bounds.center = targetPosition;
            targetPosition = _cardsTableBounds.Clamp(bounds);
            Play(targetPosition, duration, onComplete);
        }

        public override void Stop()
        {
            _animation?.Kill();
            _isPlaying.Value = false;
        }

        private Vector3 ValidatePosition(Vector3 position)
        {
            Vector3 pos = position;
            pos.y = _cardData.BaseHeight;
            return pos;
        }
    }
}
