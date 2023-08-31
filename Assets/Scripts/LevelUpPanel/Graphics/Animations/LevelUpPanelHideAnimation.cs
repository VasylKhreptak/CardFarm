using System;
using DG.Tweening;
using UnityEngine;

namespace LevelUpPanel.Graphics.Animations
{
    public class LevelUpPanelHideAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _curve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Action onComplete = null)
        {
            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .Append(_canvasGroup.DOFade(0f, _duration).SetEase(_curve))
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
