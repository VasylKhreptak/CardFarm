using System;
using Cards.Data;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Cards.Graphics.Animations
{
    public class CardMoveAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private AnimationCurve _curve;

        private Tween _tween;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
        }
        
        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            _cardData.UnlinkFromUpper();

            Stop();
            targetPosition = ValidatePosition(targetPosition);

            _tween = _cardData.transform.DOMove(targetPosition, duration)
                .SetEase(_curve)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .Play();
        }

        public void Stop()
        {
            _tween?.Kill();
        }

        private Vector3 ValidatePosition(Vector3 position)
        {
            Vector3 pos = position;
            pos.y = _cardData.BaseHeight;
            return pos;
        }
    }
}
