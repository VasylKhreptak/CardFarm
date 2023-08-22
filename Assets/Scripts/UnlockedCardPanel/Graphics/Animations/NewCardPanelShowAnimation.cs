using System;
using DG.Tweening;
using NaughtyAttributes;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace UnlockedCardPanel.Graphics.Animations
{
    public class NewCardPanelShowAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;

        [Header("General Preferences")]
        [SerializeField] private float _duration = 1f;

        [Header("Anchor Position Preferences")]
        [SerializeField] private Vector2 _endAnchorPos;
        [SerializeField] private AnimationCurve _anchorPosCurve;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        [Header("Background Fade Preferences")]
        [SerializeField] private float _startBackgroundAlpha;
        [SerializeField] private float _endBackgroundAlpha;
        [SerializeField] private AnimationCurve _backgroundFadeCurve;

        [Header("Size Delta Preferences")]
        [SerializeField] private Vector2 _startSizeDelta;
        [SerializeField] private Vector2 _endSizeDelta;
        [SerializeField] private AnimationCurve _sizeDeltaCurve;

        [Header("Hook Preferences")]
        [SerializeField] private Vector2 _hookOffset = new Vector2(0f, 300f);
        [SerializeField] private float _hookMoveDuration = 0.3f;
        [SerializeField] private AnimationCurve _hookMoveCurve;
        
        private Sequence _sequence;

        private bool _isPlaying = false;

        public bool IsPlaying => _isPlaying;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            SetStartValues();

            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            Stop();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        public void Play(Vector2 startAnchorPos, Action onMovedToHookPosition = null)
        {
            Stop();

            float totalDuration = _duration + _hookMoveDuration;

            _sequence = DOTween.Sequence();

            _rectTransform.anchoredPosition = startAnchorPos;

            Sequence moveSequence = DOTween.Sequence();

            moveSequence
                .Append(_rectTransform.DOAnchorPos(startAnchorPos + _hookOffset, _hookMoveDuration).SetEase(_hookMoveCurve))
                .AppendCallback(() => onMovedToHookPosition?.Invoke())
                .Append(_rectTransform.DOAnchorPos(_endAnchorPos, _duration).SetEase(_anchorPosCurve));

            _sequence
                .OnPlay(() => _isPlaying = true)
                .Join(_rectTransform.DOScale(_endScale, totalDuration).SetEase(_scaleCurve))
                .Join(_rectTransform.DOSizeDelta(_endSizeDelta, totalDuration).SetEase(_sizeDeltaCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, totalDuration).SetEase(_fadeCurve))
                .Join(moveSequence)
                .Join(_backgroundCanvasGroup.DOFade(_endBackgroundAlpha, totalDuration).SetEase(_backgroundFadeCurve))
                .OnComplete(() => _isPlaying = false)
                .OnKill(() => _isPlaying = false)
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence?.Kill();
        }

        private void SetStartValues()
        {
            _rectTransform.localScale = _startScale;
            _canvasGroup.alpha = _startAlpha;
            _rectTransform.sizeDelta = _startSizeDelta;
            _backgroundCanvasGroup.alpha = _startBackgroundAlpha;
        }

        [Button()]
        private void AssignEndPos()
        {
            _endAnchorPos = _rectTransform.anchoredPosition;
        }

        [Button()]
        private void AssignStartSizeDelta()
        {
            _startSizeDelta = _rectTransform.sizeDelta;
        }

        [Button()]
        private void AssignEndSizeDelta()
        {
            _endSizeDelta = _rectTransform.sizeDelta;
        }

        [Button()]
        private void AssignStartScale()
        {
            _startScale = _rectTransform.localScale;
        }

        [Button()]
        private void AssignEndScale()
        {
            _endScale = _rectTransform.localScale;
        }

        private void OnRestart()
        {
            SetStartValues();
        }
    }
}
