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

        [Header("Move Preferences")]
        [SerializeField] private Vector2 _centerAnchoredPosition;
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("General Preferences")]
        [SerializeField] private float _duration = 1f;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

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

        public void Play(Vector2 startAnchoredPosition, Action onComplete = null)
        {
            Stop();

            _rectTransform.anchoredPosition = startAnchoredPosition;

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Join(_rectTransform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(_rectTransform.DOAnchorPos(_centerAnchoredPosition, _duration).SetEase(_moveCurve))
                .OnPlay(() =>
                {
                    _isPlaying = true;
                })
                .OnComplete(() =>
                {
                    _isPlaying = false;
                    onComplete?.Invoke();
                })
                .OnKill(() =>
                {
                    _isPlaying = false;
                })
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence?.Kill();
            _isPlaying = false;
        }

        private void SetStartValues()
        {
            _rectTransform.localScale = _startScale;
            _canvasGroup.alpha = _startAlpha;
            _rectTransform.anchoredPosition = _centerAnchoredPosition;
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
            Stop();
            SetStartValues();
        }
    }
}
