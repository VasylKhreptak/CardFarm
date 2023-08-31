using System;
using Graphics.Animations;
using Quests.Logic;
using Quests.Logic.Core;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop.Buttons
{
    public class OpenShopButtonHighlighter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FadeHighlighter _fadeHighlighter;
        [SerializeField] private Button _openButton;

        [Header("Preferences")]
        [SerializeField] private Quest _startAfterQuestReward;

        private IDisposable _questSubscription;

        private bool _opened = false;
        private bool _wasHighlighted = false;

        private QuestsManager _questsManager;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            GameRestartCommand gameRestartCommand)
        {
            _questsManager = questsManager;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;

            StartObservingQuests();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;

            StopObservingQuests();

            _fadeHighlighter.StopHighlighting();

            _openButton.onClick.RemoveListener(OnClick);
            _wasHighlighted = false;
            _opened = false;
        }

        #endregion

        private void OnClick()
        {
            if (_wasHighlighted == false) return;

            _fadeHighlighter.StopHighlighting();
            _opened = true;
            StopObservingQuests();
        }

        private void StartObservingQuests()
        {
            StopObservingQuests();

            foreach (var possibleQuest in _questsManager.RewardedQuests)
            {
                if (possibleQuest.Quest == _startAfterQuestReward)
                {
                    StartHighlighting();
                }
            }

            _questSubscription = _questsManager.RewardedQuests.ObserveAdd().Subscribe(addEvent =>
            {
                OnRewardedQuest(addEvent.Value.Quest);
            });
        }

        private void StopObservingQuests()
        {
            _questSubscription?.Dispose();
        }

        private void OnRewardedQuest(Quest quest)
        {
            if (quest == _startAfterQuestReward)
            {
                StartHighlighting();
            }
        }

        private void StartHighlighting()
        {
            _fadeHighlighter.StartHighlighting();
            _openButton.onClick.RemoveListener(OnClick);
            _openButton.onClick.AddListener(OnClick);
            _wasHighlighted = true;
        }

        private void OnRestart()
        {
            _wasHighlighted = false;
            _opened = false;
            StartObservingQuests();
        }
    }
}
