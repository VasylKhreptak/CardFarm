using DG.Tweening;
using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace Graphics.Animations.UI
{
    public class ClickAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _startAlpha;
        [SerializeField] private float _targetAlpha;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _fadeCurve;

        private Sequence _sequence;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            Stop();
            ResetValues();
        }

        #endregion

        public void Play(Vector3 spawnScreenPosition, Vector3 linkToWorldPosition)
        {
            Stop();
            ResetValues();

            Tween scaleTween = _transform.DOScale(_targetScale, _duration).SetEase(_scaleCurve);
            Tween fadeTween = _canvasGroup.DOFade(_targetAlpha, _duration).SetEase(_fadeCurve);

            _transform.position = spawnScreenPosition;

            _sequence = DOTween.Sequence()
                .Append(scaleTween)
                .Join(fadeTween)
                .OnComplete(() => gameObject.SetActive(false))
                .OnUpdate(() =>
                {
                    Vector3 screenPosition = _camera.WorldToScreenPoint(linkToWorldPosition);
                    _transform.position = screenPosition;
                })
                .Play();
        }

        public void Stop()
        {
            KillSequence();
            ResetValues();
        }

        private void KillSequence()
        {
            _sequence?.Kill();
        }

        private void ResetValues()
        {
            ResetScale();
            ResetAlpha();
        }

        private void ResetScale()
        {
            _transform.localScale = _startScale;
        }

        private void ResetAlpha()
        {
            _canvasGroup.alpha = _startAlpha;
        }
    }
}
