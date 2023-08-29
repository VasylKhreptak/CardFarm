using System;
using Cards.Data;
using Cards.Graphics.Animations.Core;
using DG.Tweening;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class NewCardAppearAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;
        
        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha = 1f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Flip Preferences")]
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;

        [Header("Raise Preferences")]
        [SerializeField] private float _raiseDuration = 0.7f;
        [SerializeField] private float _raiseHeight = 0.6f;
        [SerializeField] private Vector3 _startScale = Vector3.one * 0.4f;
        [SerializeField] private Vector3 _raiseScale;
        [SerializeField] private AnimationCurve _raiseHeightCurve;
        [SerializeField] private AnimationCurve _raiseScaleCurve;

        [Header("Place Preferences")]
        [SerializeField] private float _placeDuration;
        [SerializeField] private AnimationCurve _placeHeightCurve;
        [SerializeField] private AnimationCurve _placeScaleCurve;

        public event Action onRaised;

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
                .Join(_cardData.CanvasGroup.DOFade(_endAlpha, _fadeDuration).SetEase(_fadeCurve))
                .Join(_cardData.transform.DOMoveY(_raiseHeight, _raiseDuration).SetEase(_raiseHeightCurve))
                .Join(_cardData.transform.DOScale(_raiseScale, _raiseDuration).SetEase(_raiseScaleCurve))
                .AppendCallback(() => onRaised?.Invoke())
                .OnPlay(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnUpdate(() =>
                {
                    Vector3 position = _cardData.transform.position;
                    _cardData.Height.Value = position.y;
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _cardData.RenderOnTop();
                })
                .OnComplete(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
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

        public void PlaceCardOnTable(Action onComplete = null)
        {
            Stop();
            _sequence = DOTween.Sequence();

            _sequence
                .Append(_cardData.transform.DOMoveY(_cardData.BaseHeight, _placeDuration).SetEase(_placeHeightCurve))
                .Join(_cardData.transform.DOScale(Vector3.one, _placeDuration).SetEase(_placeScaleCurve))
                .OnPlay(() =>
                {
                    _isPlaying.Value = true;
                })
                .OnUpdate(() =>
                {
                    _cardData.Height.Value = _cardData.transform.position.y;
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _cardData.RenderOnTop();
                })
                .OnComplete(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    onComplete?.Invoke();
                    _isPlaying.Value = false;
                })
                .OnKill(() =>
                {
                    _cardData.NewCardShirtStateUpdater.UpdateCullState();
                    _isPlaying.Value = false;
                })
                .Play();
        }

        public void HideCardSuit()
        {
            _cardData.transform.localRotation = Quaternion.Euler(_endLocalRotation);
            _cardData.NewCardShirtStateUpdater.UpdateCullState();
        }
    }
}
