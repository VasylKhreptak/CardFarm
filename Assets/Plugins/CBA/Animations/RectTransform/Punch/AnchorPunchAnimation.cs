using System;
using CBA.Animations.RectTransform.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.RectTransform.Punch
{
    public class AnchorPunchAnimation : RectTransformAnimation
    {
        [Header("Punch Preferences")]
        [SerializeField] protected Vector2 _strengthDirection;
        [SerializeField] protected float _strength = 5f;
        [SerializeField] protected int _vibrato = 10;
        [SerializeField] protected float _elasticity = 1f;

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
            return _rectTransform.DOPunchAnchorPos(_strengthDirection * _strength, _duration, _vibrato, _elasticity, _snapping);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rectTransform.DOPunchAnchorPos(-_strengthDirection * _strength, _duration, _vibrato, _elasticity, _snapping);
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

            DrawPunchStrength();
        }

        private void DrawPunchStrength()
        {
            ///
        }

#endif

        #endregion

    }
}
