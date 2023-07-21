using Plugins.ObjectPooler.Example;
using UnityEngine;
using Zenject;

namespace Graphics.Particles.Logic
{
    public class ParticleOnDisable : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private MainPool _particle;
        [SerializeField] private Vector3 _spawnOffset;

        private MainObjectPooler _objectPooler;
        private int _enabledFrameCount;

        [Inject]
        private void Constructor(MainObjectPooler objectPooler)
        {
            _objectPooler = objectPooler;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnEnable()
        {
            _enabledFrameCount = Time.frameCount;
        }

        private void OnDisable()
        {
            if (_enabledFrameCount == Time.frameCount) return;

            if (_objectPooler == null) return;

            SpawnParticle();
        }

        #endregion

        private void SpawnParticle()
        {
            Vector3 position = _transform.position + _spawnOffset;

            _objectPooler.Spawn(_particle, position);
        }
    }
}
