using System;
using Data.Player.Core;
using Data.Player.Experience;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Experience
{
    public class ExperienceLevelText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _experienceLevelSubscription;

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
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

            _experienceLevelSubscription = _experienceData.ExperienceLevel.Subscribe(SetText);
        }

        private void StopObserving()
        {
            _experienceLevelSubscription?.Dispose();
        }

        private void SetText(int experienceLevel)
        {
            _tmp.text = experienceLevel.ToString();
        }
    }
}
