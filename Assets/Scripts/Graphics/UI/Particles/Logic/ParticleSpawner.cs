using System;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Data;
using Providers.Graphics;
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

        [Inject]
        private void Constructor(ParticlePooler particlePooler, CameraProvider cameraProvider)
        {
            _particlePooler = particlePooler;
            _camera = cameraProvider.Value;
        }

        public ParticleData Spawn(Particle particle)
        {
            GameObject spawnedParticle = _particlePooler.Spawn(particle);
            ParticleData particleData = spawnedParticle.GetComponent<ParticleData>();
            particleData.RectTransform.position = Vector2.zero;
            particleData.RectTransform.localScale = Vector2.one;
            particleData.RectTransform.rotation = Quaternion.LookRotation(-_camera.transform.forward);
            particleData.RectTransform.SetAsLastSibling();
            OnSpawned?.Invoke(particle);
            return particleData;
        }

        public ParticleData Spawn(Particle particle, Vector3 position)
        {
            ParticleData particleData = Spawn(particle);
            particleData.RectTransform.position = position;
            return particleData;
        }

        public ParticleData SpawnInRange(Particle particle, Vector3 position, float range)
        {
            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized;
            Vector3 direction = new Vector3(insideUnitCircle.x, insideUnitCircle.y, 0);
            Vector3 targetPosition = position + direction * range;
            ParticleData particleData = Spawn(particle, targetPosition);
            return particleData;
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
