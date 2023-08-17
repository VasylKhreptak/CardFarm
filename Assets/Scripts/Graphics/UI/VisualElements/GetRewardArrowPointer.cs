using System;
using Graphics.Animations.UI;
using Quests.Logic;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.VisualElements
{
    public class GetRewardArrowPointer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ArrowPointerAnimation _arrowPointerAnimation;

        private IDisposable _completedQuestsSubscription;

        private QuestsManager _questsManager;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(QuestsManager questsManager, GameRestartCommand gameRestartCommand)
        {
            _questsManager = questsManager;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnValidate()
        {
            _arrowPointerAnimation ??= GetComponentInChildren<ArrowPointerAnimation>(true);
        }

        private void OnEnable()
        {
            StartObservingQuests();
        }

        private void OnDisable()
        {
            StopObservingQuests();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObservingQuests()
        {
            StopObservingQuests();

            _completedQuestsSubscription = _questsManager.CompletedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(OnCompletedQuestsChanged)
                .Subscribe(_ => OnCompletedQuestsChanged());
        }

        private void StopObservingQuests()
        {
            _completedQuestsSubscription?.Dispose();
        }

        private void OnCompletedQuestsChanged()
        {
            int completedQuestsCount = _questsManager.CompletedQuests.Count;

            if (completedQuestsCount == 1)
            {
                _arrowPointerAnimation.Play();
            }
            else
            {
                _arrowPointerAnimation.Stop();
            }
        }

        private void OnRestart()
        {
            _arrowPointerAnimation.gameObject.SetActive(false);
        }
    }
}
