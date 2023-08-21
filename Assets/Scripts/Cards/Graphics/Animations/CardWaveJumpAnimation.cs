using System;
using System.Collections.Generic;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardWaveJumpAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _interval;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _attenuation = 0.10f;

        private IDisposable _isAnyCardSelectedSubscription;

        private Sequence _sequence;
        private Sequence _singelJumpSequence;

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
            _isAnyCardSelectedSubscription = _cardData.IsAnyGroupCardSelected.Subscribe(_ =>
            {
                Stop();
                _cardData.HeightController.enabled = true;
            });
        }

        private void OnDisable()
        {
            Stop();
            _cardData.HeightController.enabled = true;

            _isAnyCardSelectedSubscription?.Dispose();
        }

        #endregion

        [Button()]
        public void Play(int loops = 1, Action onLoopPlay = null)
        {
            Stop();

            List<CardData> groupCards = _cardData.GroupCards;

            if (groupCards.Count == 0) return;

            _sequence = DOTween.Sequence();

            float delay = 0;
            float jumpHeight = _jumpHeight;
            float duration = _duration;

            for (int i = groupCards.Count - 1; i >= 0; i--)
            {
                CardData groupCard = groupCards[i];

                groupCard.HeightController.ResetHeight();
                _sequence.Join(groupCard.Animations.WaveJumpAnimation.CreateJumpSequence(jumpHeight, duration, delay));

                delay += _interval;
                jumpHeight -= jumpHeight * _attenuation;
                duration -= duration * _attenuation;
            }

            _sequence
                .AppendCallback(() => onLoopPlay?.Invoke())
                .OnPlay(() => SetActiveHeightControllers(false))
                .OnComplete(() => SetActiveHeightControllers(true))
                .OnKill(() => SetActiveHeightControllers(true))
                .SetLoops(loops, LoopType.Restart)
                .Play();
        }

        [Button()]
        public override void Stop()
        {
            _sequence?.Kill();
            _singelJumpSequence?.Kill();
        }

        public Sequence CreateJumpSequence(float jumpHeight, float duration, float delay)
        {
            _singelJumpSequence?.Kill();

            _singelJumpSequence = DOTween.Sequence();

            float startHeight = _cardData.transform.position.y;

            _singelJumpSequence
                .Join(_cardData.transform.DOMoveY(startHeight + jumpHeight, duration / 2).SetEase(Ease.Linear))
                .Append(_cardData.transform.DOMoveY(startHeight, duration / 2).SetEase(Ease.Linear))
                .SetEase(_jumpCurve)
                .SetDelay(delay)
                .OnPlay(() => _isPlaying.Value = true)
                .OnUpdate(() => _cardData.Height.Value = _cardData.transform.position.y)
                .OnKill(() => _isPlaying.Value = false)
                .OnComplete(() => _isPlaying.Value = false);

            return _singelJumpSequence;
        }

        private void SetActiveHeightControllers(bool enabled)
        {
            List<CardData> groupCards = _cardData.GroupCards;

            foreach (var groupCard in groupCards)
            {
                groupCard.HeightController.enabled = enabled;
            }
        }

        private void SetHeight(float height)
        {
            Vector3 position = _cardData.transform.position;
            position.y = height;
            _cardData.Height.Value = height;
            _cardData.transform.position = position;
        }
    }
}
