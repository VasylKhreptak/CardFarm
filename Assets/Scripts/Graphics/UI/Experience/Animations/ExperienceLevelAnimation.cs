using System;
using CBA.Animations.Sequences.Core;
using Data.Player.Core;
using Data.Player.Experience;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Experience.Animations
{
    public class ExperienceLevelAnimation : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private AnimationSequence _animation;

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
            _animation ??= GetComponent<AnimationSequence>();
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

            _subscription = _experienceData.ExperienceLevel
                .Buffer(2, 1)
                .Where(x => x[1] > x[0])
                .Subscribe(_ =>
                {
                    _animation.Stop();
                    _animation.MoveToStartState();
                    _animation.PlayForwardImmediate();
                });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }
    }

}
