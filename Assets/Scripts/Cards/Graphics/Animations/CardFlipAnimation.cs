using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardFlipAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Flip Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private Ease _flipEase;

        private Tween _animation;

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

        public void Play(Action onComplete = null)
        {
            Play(_duration, onComplete);
        }

        public void Play(float duration, Action onComplete = null)
        {
            _cardData.UnlinkFromUpper();

            Stop();

            _cardData.transform.localRotation = Quaternion.Euler(_startLocalRotation);
            _animation = _cardData.transform
                .DOLocalRotate(_endLocalRotation, duration)
                .SetEase(_flipEase)
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

        public void Stop()
        {
            _animation?.Kill();
            _isPlaying.Value = false;
        }
    }
}
