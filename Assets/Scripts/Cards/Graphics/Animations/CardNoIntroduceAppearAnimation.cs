using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardNoIntroduceAppearAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha = 1f;
        [SerializeField] private float _fadeDuration = 0.2f;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Flip Preferences")]
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private float _flipDuration = 0.3f;

        [Header("Raise Preferences")]
        [SerializeField] private float _raiseHeight = 0.6f;
        [SerializeField] private AnimationCurve _raiseCurve;
        [SerializeField] private float _raiseDuration = 0.5f;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale = Vector3.one * 0.4f;
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        [SerializeField] private float _scaleDuration = 0.5f;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Place Height Preferences")]
        [SerializeField] private float _placeDuration;
        [SerializeField] private AnimationCurve _placeHeightCurve;

        [Header("Place Scale Preferences")]
        [SerializeField] private float _placeScaleDuration;
        [SerializeField] private AnimationCurve _placeScaleCurve;

        [Header("Show Preferences")]
        [SerializeField] private float _showDuration = 0.3f;

        private Sequence _sequence;

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
            SetScale(Vector3.one);
            SetLocalRotation(_endLocalRotation);
        }

        #endregion

        public void Play(Action onComplete = null)
        {
            _cardData.UnlinkFromUpper();

            Stop();
            SetAlpha(_startAlpha);
            SetScale(_startScale);
            SetLocalRotation(_startLocalRotation);

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_cardData.CanvasGroup.DOFade(_endAlpha, _fadeDuration).SetEase(_fadeCurve))
                .Join(_cardData.transform.DOMoveY(_raiseHeight, _raiseDuration).SetEase(_raiseCurve))
                .Join(_cardData.transform.DOScale(_targetScale, _scaleDuration).SetEase(_scaleCurve))
                .AppendInterval(_showDuration)
                .Append(_cardData.transform.DOLocalRotate(_endLocalRotation, _flipDuration))
                .Append(_cardData.transform.DOMoveY(_cardData.BaseHeight, _placeDuration).SetEase(_placeHeightCurve))
                .Join(_cardData.transform.DOScale(Vector3.one, _placeScaleDuration).SetEase(_placeScaleCurve))
                .OnPlay(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _isPlaying.Value = true;
                })
                .OnUpdate(() => _cardData.NewCardShirtStateUpdater.UpdateCullState())
                .OnKill(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _isPlaying.Value = false;
                })
                .OnComplete(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _isPlaying.Value = false;
                    onComplete?.Invoke();
                })
                .Play();
        }

        public override void Stop()
        {
            _sequence?.Kill();
            _isPlaying.Value = false;
        }

        private void SetScale(Vector3 scale) => _cardData.transform.localScale = scale;

        private void SetAlpha(float alpha) => _cardData.CanvasGroup.alpha = alpha;

        private void SetLocalRotation(Vector3 rotation) => _cardData.transform.localEulerAngles = rotation;
    }
}
