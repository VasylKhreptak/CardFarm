using Cards.Data;
using Plugins.ObjectPooler.Example;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Particles
{
    public class CardParticleOnLand : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private MainPool _particle = MainPool.CardLandParticle;

        private MainObjectPooler _mainObjectPooler;

        [Inject]
        private void Constructor(MainObjectPooler mainObjectPooler)
        {
            _mainObjectPooler = mainObjectPooler;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            _cardData.Callbacks.onLanded += SpawnParticle;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onLanded -= SpawnParticle;
        }

        #endregion

        private void SpawnParticle()
        {
            _mainObjectPooler.Spawn(_particle, _cardData.transform.position, Quaternion.identity);
        }
    }
}
