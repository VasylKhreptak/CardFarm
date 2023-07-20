using System;
using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Data;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Graphics.UI.Particles.Logic
{
    public class ParticleSpawner : MonoBehaviour
    {
        private ParticlePooler _particlePooler;

        public event Action<Particle> OnSpawned;

        [Inject]
        private void Constructor(ParticlePooler particlePooler)
        {
            _particlePooler = particlePooler;
        }

        public ParticleData Spawn(Particle particle)
        {
            GameObject spawnedParticle = _particlePooler.Spawn(particle);
            ParticleData particleData = spawnedParticle.GetComponent<ParticleData>();
            particleData.RectTransform.position = Vector2.zero;
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
            return SpawnInRange(particle, position, Random.Range(0, maxRange));
        }

        public ParticleData SpawnInRandomRange(Particle particle, Vector3 position, float minRange, float maxRange)
        {
            float range = Random.Range(minRange, maxRange);
            return SpawnInRange(particle, position, range);
        }
    }
}
