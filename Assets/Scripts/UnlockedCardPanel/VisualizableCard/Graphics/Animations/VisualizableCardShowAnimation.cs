using System;
using DG.Tweening;
using NaughtyAttributes;
using Providers.Graphics;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Data;
using Zenject;

namespace UnlockedCardPanel.VisualizableCard.Graphics.Animations
{
    public class VisualizableCardShowAnimation : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private VisualizableCardData _cardData;

        [Header("Flip Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Vector3 _startLocalRotation;
        [SerializeField] private Vector3 _endLocalRotation;
        [SerializeField] private AnimationCurve _flipCurve;

        [Header("Scale Preferences")]
        [SerializeField] private float _scaleDuration = 0.3f;
        [SerializeField] private Vector3 _initialScale;
        [SerializeField] private Vector3 _targetScale = new Vector3(1.15f, 1.15f, 1.15f);
        [SerializeField] private AnimationCurve _scaleCurve;

        public event Action OnComplete;

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
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<VisualizableCardData>(true);
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        [Button()]
        public void Play(float delay = 0f)
        {
            Stop();

            _cardData.transform.localRotation = Quaternion.Euler(_startLocalRotation);
            _cardData.transform.localScale = _initialScale;

            _sequence = DOTween.Sequence();

            _sequence
                .SetDelay(delay)
                .Append(_cardData.transform.DOScale(_targetScale, _scaleDuration).SetEase(_scaleCurve))
                .Append(_cardData.transform.DOLocalRotate(_endLocalRotation, _duration).SetEase(_flipCurve))
                .Append(_cardData.transform.DOScale(_initialScale, _scaleDuration).SetEase(_scaleCurve))
                .OnPlay(_cardData.ShirtCuller.UpdateCullState)
                .OnUpdate(_cardData.ShirtCuller.UpdateCullState)
                .OnComplete(() =>
                {
                    _cardData.ShirtCuller.UpdateCullState();
                    OnComplete?.Invoke();
                })
                .OnKill(_cardData.ShirtCuller.UpdateCullState)
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
