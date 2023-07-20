using Graphics.UI.Particles.Data;
using Graphics.UI.Particles.Graphics.Animations;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Particles.Logic
{
    public class ParticleDataFiller : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ParticleData _particleData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _particleData = GetComponentInParent<ParticleData>(true);

            if (_particleData == null) return;

            _particleData.Animations.MoveSequence = GetComponentInChildren<ParticleMoveSequence>(true);
        }

        #endregion
    }
}
