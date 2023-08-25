using Data.Player.Core;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Data.Player.Experience.Logic
{
    public class ExperienceController : MonoBehaviour
    {
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _experienceData.Experience.Subscribe(_ => UpdateExperienceData()).AddTo(_subscriptions);
            _experienceData.MaxExperience.Subscribe(_ => UpdateExperienceData()).AddTo(_subscriptions);
            _experienceData.ExperienceLevel.Subscribe(_ => UpdateExperienceData()).AddTo(_subscriptions);
            _experienceData.TotalExperience.Subscribe(_ => UpdateExperienceData()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void UpdateExperienceData()
        {
            ClampExperience();

            UpdateExperienceLevel();
        }

        private void ClampExperience()
        {
            if (_experienceData.Experience.Value < 0)
            {
                _experienceData.Experience.Value = 0;
            }
            else if (_experienceData.Experience.Value > _experienceData.MaxExperience.Value)
            {
                _experienceData.Experience.Value = _experienceData.MaxExperience.Value;
            }

            if (_experienceData.MaxExperience.Value < 0)
            {
                _experienceData.MaxExperience.Value = 1;
            }

            if (_experienceData.TotalExperience.Value < 0)
            {
                _experienceData.TotalExperience.Value = 0;
            }
        }

        private void UpdateExperienceLevel()
        {
            int maxExperience = _experienceData.MaxExperience.Value;
            int totalExperience = _experienceData.TotalExperience.Value;

            _experienceData.Experience.Value = totalExperience % maxExperience;
            _experienceData.ExperienceLevel.Value = totalExperience / maxExperience;

            _experienceData.FillAmount.Value = (float)_experienceData.Experience.Value / _experienceData.MaxExperience.Value;
        }

        [Button]
        private void Add30Experience()
        {
            _experienceData.TotalExperience.Value += 30;
        }

        [Button]
        private void Add100Experience()
        {
            _experienceData.TotalExperience.Value += 100;
        }

        [Button()]
        private void Remove30Experience()
        {
            _experienceData.TotalExperience.Value -= 30;
        }
    }
}
