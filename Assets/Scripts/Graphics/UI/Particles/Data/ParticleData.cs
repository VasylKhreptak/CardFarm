using Graphics.UI.Particles.Core;
using UnityEngine;

namespace Graphics.UI.Particles.Data
{
    public class ParticleData : MonoBehaviour
    {
        [SerializeField] private Particle _particle;
        [SerializeField] private RectTransform _rectTransform;

        public Particle Particle => _particle;
        public RectTransform RectTransform => _rectTransform;

        public ParticleAnimations Animations = new ParticleAnimations();
    }
}
