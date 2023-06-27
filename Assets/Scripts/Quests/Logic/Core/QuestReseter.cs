using System;
using Quests.Data;
using UniRx;
using UnityEngine;

namespace Quests.Logic.Core
{
    public class QuestReseter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        private IDisposable _tookRewardSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _questData ??= GetComponent<QuestData>();
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
            StartObservingTookReward();
        }

        private void StopObserving()
        {
            StopObservingTookReward();
        }

        private void StartObservingTookReward()
        {
            StopObservingTookReward();
            _tookRewardSubscription = _questData.TookReward.Subscribe(OnTookRewardUpdated);
        }

        private void StopObservingTookReward()
        {
            _tookRewardSubscription?.Dispose();
        }

        private void OnTookRewardUpdated(bool tookReward)
        {
            if (tookReward)
            {
                _questData.gameObject.SetActive(false);
                _questData.IsCompleted.Value = false;
                _questData.TookReward.Value = false;
            }
        }
    }
}
