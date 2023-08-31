using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LevelUpPanel.Buttons
{
    public class WatchAddButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _buttonObject;
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animation Preferences")]
        [SerializeField] private float _duration;

        [Header("Scale Preferences")]
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private AnimationCurve _scaleCurve;

        [Header("Fade Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _endAlpha;
        [SerializeField] private AnimationCurve _fadeCurve;

        private bool _watchedAd = false;

        private Sequence _sequence;

        public event Action OnClicked;

        public event Action OnWatchedAd;

        #region MonoBehaviour

        private void Awake()
        {
            ResetValue();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnCLicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnCLicked);

            ResetValue();

            _watchedAd = false;
        }

        #endregion

        public void Show()
        {
            KillSequence();

            Enable();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_buttonObject.transform.DOScale(_endScale, _duration).SetEase(_scaleCurve))
                .Join(_canvasGroup.DOFade(_endAlpha, _duration).SetEase(_fadeCurve))
                .Play();
        }

        public void Hide()
        {
            KillSequence();

            _sequence = DOTween.Sequence();

            _sequence
                .Join(_buttonObject.transform.DOScale(_startScale, _duration).SetEase(_scaleCurve))
                .Join(_canvasGroup.DOFade(_startAlpha, _duration).SetEase(_fadeCurve))
                .OnComplete(Disable)
                .Play();
        }

        private void Enable() => _buttonObject.SetActive(true);

        private void Disable() => _buttonObject.SetActive(false);

        private void ResetValue()
        {
            SetScale(_startScale);
            SetAlpha(_startAlpha);
        }

        private void SetScale(Vector3 scale) => _buttonObject.transform.localScale = scale;

        private void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;

        private void KillSequence() => _sequence?.Kill();

        private void OnCLicked()
        {
            if (_watchedAd) return;

            OnClicked?.Invoke();
            OnWatchedAd?.Invoke();

            _watchedAd = true;
        }
    }
}
