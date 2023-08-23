using System;
using Extensions;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Data;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Graphics.UI.Particles.Logic
{
    public class ParticleSpawner : MonoBehaviour
    {
        public event Action<Particle> OnSpawned;

        private ParticlePooler _particlePooler;
        private Camera _camera;
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        [Inject]
        private void Constructor(ParticlePooler particlePooler, CanvasProvider canvasProvider)
        {
            _particlePooler = particlePooler;
            _canvas = canvasProvider.Value;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _camera = _canvas.worldCamera;
        }

        public ParticleData Spawn(Particle particle)
        {
            GameObject spawnedParticle = _particlePooler.Spawn(particle);
            ParticleData particleData = spawnedParticle.GetComponent<ParticleData>();
            particleData.RectTransform.localPosition = Vector3.zero;
            particleData.RectTransform.localScale = Vector3.one;
            particleData.RectTransform.forward = _camera.transform.forward;
            particleData.RectTransform.SetAsLastSibling();
            OnSpawned?.Invoke(particle);
            return particleData;
        }

        public ParticleData Spawn(Particle particle, Vector3 position)
        {
            ParticleData particleData = Spawn(particle);

            Vector3 anchoredPosition = particleData.RectTransform.GetAnchoredPosition(_camera, position);

            particleData.RectTransform.anchoredPosition3D = anchoredPosition;
            return particleData;
        }

        public ParticleData SpawnInRange(Particle particle, Vector3 position, float range)
        {
            Vector3 insideUnitCircle = Random.insideUnitSphere * range;

            Vector3 targetPosition = position + insideUnitCircle;

            return Spawn(particle, targetPosition);
        }

        public ParticleData SpawnInRandomRange(Particle particle, Vector3 position, float maxRange)
        {
            return SpawnInRandomRange(particle, position, 0, maxRange);
        }

        public ParticleData SpawnInRandomRange(Particle particle, Vector3 position, float minRange, float maxRange)
        {
            float range = Random.Range(minRange, maxRange);
            return SpawnInRange(particle, position, range);
        }
    }
}
