using System;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove
{
    public class CameraMovableStateController : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _allowToMoveAfterQuestNumber;

        private IDisposable _questCountSubscription;

        private QuestsManager _questsManager;
        private CameraMover _cameraMover;

        [Inject]
        private void Constructor(QuestsManager questsManager, CameraMover cameraMover)
        {
            _questsManager = questsManager;
            _cameraMover = cameraMover;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _cameraMover.CanMove = false;
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
            _questCountSubscription = _questsManager.FinishedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(() => OnFinishedQuestsCountChanged(_questsManager.FinishedQuests.Count))
                .Subscribe(OnFinishedQuestsCountChanged);
        }

        private void StopObserving()
        {
            _questCountSubscription?.Dispose();
        }

        private void OnFinishedQuestsCountChanged(int count)
        {
            _cameraMover.CanMove = count >= _allowToMoveAfterQuestNumber;
        }
    }
}
