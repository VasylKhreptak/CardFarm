using System;
using DG.Tweening;
using TreasureChests.Data;
using UnityEngine;
using Zenject;

namespace TreasureChests.Graphics.Animations
{
    public class ChestSpinAnimation : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private TreasureChestData _chestData;

        [Header("Preferences")]
        [SerializeField] private float _duration;
        [SerializeField] private Vector3 _axis;
        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private AnimationCurve _spinCurve;

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<TreasureChestData>(true);
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        public void Play(int spinCount, Action onComplete = null) => Play(spinCount, _duration, onComplete);

        public void Play(int spinCount, float duration, Action onComplete = null)
        {
            Stop();

            _chestData.transform.localEulerAngles = _startRotation;

            _sequence = DOTween.Sequence();

            _sequence
                .Append(DOTween
                    .Sequence()
                    .Append(_chestData.transform
                        .DOLocalRotate(_chestData.transform.localEulerAngles + _axis * 360f, duration / spinCount, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear))
                    .SetLoops(spinCount, LoopType.Restart)
                )
                .SetEase(_spinCurve)
                .Play();
        }

        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}
