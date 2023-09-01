using Cards.Core;
using Cards.Data;
using Data.Player.Core;
using Data.Player.Experience;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardExperienceGainOnRecipeResult : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private int _experience = 1;

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
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

        private void OnSpawnedRecipeResult(CardData card) => GainExperience();

        private void GainExperience()
        {
            _experienceData.TotalExperience.Value += _experience;
        }
    }
}
