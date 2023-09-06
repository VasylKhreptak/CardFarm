using Cards.Data;
using Data.Player.Core;
using Data.Player.Experience;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.Grid.Logic
{
    public class CraftingGridExperienceGainer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CraftingGridCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private int _experienceGain = 20;

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
            _cardData = GetComponentInParent<CraftingGridCardData>(true);
        }

        private void OnEnable()
        {
            _cardData.OnSpawnedGridCard += OnSpawnedGridCard;
        }

        private void OnDisable()
        {
            _cardData.OnSpawnedGridCard -= OnSpawnedGridCard;
        }

        #endregion

        private void OnSpawnedGridCard(CardData cardData) => GainExperience();

        private void GainExperience()
        {
            _experienceData.TotalExperience.Value += _experienceGain;
        }
    }
}
