using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Graphics.Animations.UI.Buttons
{
    public class ButtonScaleAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Button _button;

        [Header("Preferences")]
        [SerializeField] private Vector3 _initialScale = Vector3.one;
        [SerializeField] private Vector3 _pressedScale = Vector3.one * 0.9f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _curve;

        private Tween _scaleTween;

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        private void OnDisable()
        {
            ResetScale();
            KillAnimation();
        }

        #endregion

        public void OnPointerDown(PointerEventData data)
        {
            SetScaleSmooth(_pressedScale);
        }

        public void OnPointerUp(PointerEventData data)
        {
            SetScaleSmooth(_initialScale);
        }

        private void SetScaleSmooth(Vector3 scale)
        {
            KillAnimation();

            _scaleTween = _targetTransform
                .DOScale(scale, _duration)
                .SetEase(_curve)
                .Play();
        }

        private void ResetScale()
        {
            transform.localScale = _initialScale;
        }

        private void KillAnimation()
        {
            _scaleTween?.Kill();
        }
    }
}
