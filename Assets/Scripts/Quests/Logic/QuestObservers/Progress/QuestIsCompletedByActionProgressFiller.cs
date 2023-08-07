using System;
using DG.Tweening;
using Quests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers.Progress
{
    public class QuestIsCompletedByActionProgressFiller : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        [Header("Preferences")]
        [SerializeField] private float _progressUpdateDuration = 1f;

        private IDisposable _isCompletedByActionSubscription;

        private Tween _progressTween;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _questData = GetComponentInParent<QuestData>(true);
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
            _isCompletedByActionSubscription = _questData.IsCompletedByAction.Subscribe(OnIsCompletedByActionChanged);
        }

        private void StopObserving()
        {
            _isCompletedByActionSubscription?.Dispose();
        }

        private void SetProgress(float progress)
        {
            _questData.Progress.Value = progress;
        }

        private float GetProgress() => _questData.Progress.Value;

        private void SetProgressSmooth(float progress)
        {
            KillProgressTween();

            _progressTween = DOTween
                .To(GetProgress, SetProgress, progress, _progressUpdateDuration)
                .SetEase(Ease.OutCubic)
                .Play();
        }

        private void KillProgressTween()
        {
            _progressTween?.Kill();
        }

        private void OnIsCompletedByActionChanged(bool isCompletedByAction)
        {
            if (isCompletedByAction)
            {
                SetProgressSmooth(1f);
            }
        }
    }
}
