using System;
using Cards.Data;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Cards.Graphics.Animations
{
    public class CardJumpAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Ease _mainEase;
        [SerializeField] private float _duration = 0.5f;

        [Header("Jump Preferences")]
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private Ease _jumpEase;

        [Header("Flip Preferences")]
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private Ease _flipEase;

        private Tween _animation;

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


        public void Play(Vector3 targetPosition, Action onComplete = null)
        {
            Play(targetPosition, _duration, onComplete);
        }
        
        public void Play(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            targetPosition = ValidatePosition(targetPosition);

            _cardData.UnlinkFromUpper();

            Stop();
            
            Tween jumpAnimation = _cardData.transform.DOJump(targetPosition, _jumpPower, _numberOfJumps, duration).SetEase(_jumpEase);
            _cardData.transform.localRotation = Quaternion.Euler(_startLocalRotation);
            Tween flipAnimation = _cardData.transform.DOLocalRotate(_endLocalRotation, duration).SetEase(_flipEase);

            _animation = DOTween.Sequence()
                .Append(jumpAnimation)
                .Join(flipAnimation)
                .SetEase(_mainEase)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .Play();
        }

        public void Stop()
        {
            _animation?.Kill();
        }

        private Vector3 ValidatePosition(Vector3 position)
        {
            Vector3 pos = position;
            pos.y = _cardData.BaseHeight;
            return pos;
        }
    }
}
