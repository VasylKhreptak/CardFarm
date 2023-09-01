using Cards.Core;
using Cards.Data;
using Plugins.ObjectPooler.Example;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Particles
{
    public class CardParticleOnSpawnedRecipeResult : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private Vector3 _offset = new Vector3(0, 1, 0);
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
            _cardData.Callbacks.onSpawnedRecipeResult += OnSpawnedRecipeResult;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onSpawnedRecipeResult -= OnSpawnedRecipeResult;
        }

        #endregion

        private void OnSpawnedRecipeResult(CardData card) => SpawnParticle();

        private void SpawnParticle()
        {
            _mainObjectPooler.Spawn(_particle, _cardData.GroupCenter.Value + _offset, Quaternion.identity);
        }
    }
}
