﻿using System;
using Extensions;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Logic
{
    public class ParticlesPileSpawner : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _minCount = 1;
        [SerializeField] private int _maxCount = 10;
        [SerializeField] private float _minInterval = 0.1f;
        [SerializeField] private float _maxInterval = 0.5f;
        [SerializeField] private float _minRange = 0f;
        [SerializeField] private float _maxRange = 15f;
        [SerializeField] private AnimationCurve _curve;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private ParticleSpawner _particleSpawner;

        [Inject]
        private void Constructor(ParticleSpawner particleSpawner)
        {
            _particleSpawner = particleSpawner;
        }

        #region MonoBehaivour

        private void OnDisable()
        {
            _subscriptions.Clear();
        }

        #endregion

        public void Spawn(Particle particle, int count, Vector3 startPosition, Vector3 targetPosition, Action onComplete = null)
        {
            float range = GetRange(count);
            Spawn(particle, count, startPosition, targetPosition, range, onComplete);
        }

        public void Spawn(Particle particle, int count, Vector3 startPosition, Vector3 targetPosition, float range, Action onComplete = null, Action onStart = null)
        {
            float interval = GetInterval(count);

            float delay = 0;

            for (int i = 0; i < count; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
                {
                    ParticleData particleData = _particleSpawner.SpawnInRandomRange(particle, startPosition, range);
                    particleData.Animations.MoveSequence.Play(targetPosition, onComplete);
                    onStart?.Invoke();
                }).AddTo(_subscriptions);

                delay += interval;
            }
        }

        public float GetRange(int count)
        {
            return _curve.Evaluate(_minCount, _maxCount, count, _minRange, _maxRange);
        }

        public float GetInterval(int count)
        {
            return _curve.Evaluate(_minCount, _maxCount, count, _maxInterval, _minInterval);
        }
    }
}
