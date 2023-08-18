using CBA.Animations.RectTransform.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.RectTransform.Shake
{
    public class AnchorShakeAnimation : RectTransformAnimation
    {
        [Header("Shake Preferences")]
        [SerializeField] protected Vector2 _strengthDirection;
        [SerializeField] protected float _strength = 5f;
        [SerializeField] protected int _vibrato = 10;
        [SerializeField] protected float _randomness = 90f;
        [SerializeField] protected bool _fadeOut = true;
        [SerializeField] protected ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

        [Header("Snapping")]
        [SerializeField] private bool _snapping;

        private Vector2 _startAnchoredPosition;

        #region MonoBehaviour

        private void Awake()
        {
            _startAnchoredPosition = _rectTransform.anchoredPosition;
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _rectTransform.DOShakeAnchorPos(_duration, _strengthDirection * _strength, _vibrato, _randomness, _snapping, _fadeOut, _shakeRandomnessMode);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rectTransform.DOShakeAnchorPos(_duration, -_strengthDirection * _strength, _vibrato, _randomness, _snapping, _fadeOut, _shakeRandomnessMode);

        }

        public override void MoveToStartState()
        {
            ResetAnchoredPosition();
        }

        public override void MoveToEndState()
        {
            ResetAnchoredPosition();
        }

        private void ResetAnchoredPosition()
        {
            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }

        #region Editor

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (_rectTransform == null) return;

            DrawShake();
        }

        protected virtual void DrawShake()
        {
            ///
        }

#endif

        #endregion

    }
}
