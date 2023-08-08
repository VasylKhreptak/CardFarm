﻿using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Reminder
{
    public class ScalePunchReminderAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _repeatInterval = 1f;
        [SerializeField] private Vector3 _force;
        [SerializeField] private float _duration;
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float _elasticity = 1f;
        [SerializeField] private Vector3 _initialScale = Vector3.one;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

        private Sequence _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        [Button()]
        public void Play()
        {
            Stop();

            _sequence = DOTween.Sequence();

            _sequence
                .SetDelay(_startDelay)
                .AppendCallback(() => _transform.localScale = _initialScale)
                .Append(_transform.DOPunchScale(_force, _duration, vibrato, _elasticity))
                .AppendInterval(_repeatInterval)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(_curve)
                .Play();
        }

        [Button()]
        public void Stop()
        {
            _sequence?.Kill();
        }
    }
}