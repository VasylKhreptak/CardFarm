using System;
using Data.Player.Core;
using Data.Player.Experience;
using UniRx;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.GridCells.Logic
{
    public class GridCellExperienceGainer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private GridCellCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private int _experienceGain = 20;

        private IDisposable _subscription;

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
            _cardData = GetComponentInParent<GridCellCardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _subscription = _cardData.ContainsTargetCard.Where(x => x).Subscribe(_ => GainExperience());
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void GainExperience()
        {
            _experienceData.TotalExperience.Value += _experienceGain;
        }
    }
}
