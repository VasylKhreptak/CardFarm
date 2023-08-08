﻿using System;
using Graphics.Animations.Quests.RewardPanel;
using Graphics.Animations.Reminder;
using Quests.Logic;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class GetRewardPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _quest;
        private IDisposable _nonRewardedQuestSubscription;

        [Header("Animations")]
        [SerializeField] private RewardPanelShowAnimation _showAnimation;
        [SerializeField] private RewardPanelHideAnimation _hideAnimation;
        [SerializeField] private ScalePunchReminderAnimation _scalePunchReminder;
        [SerializeField] private AnchorPositionPunchReminderAnimation _positionPunchReminder;

        private GameRestartCommand _gameRestartCommand;
        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand,
            QuestsManager questsManager)
        {
            _gameRestartCommand = gameRestartCommand;
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _showAnimation ??= GetComponentInChildren<RewardPanelShowAnimation>(true);
            _hideAnimation ??= GetComponentInChildren<RewardPanelHideAnimation>(true);
            _positionPunchReminder ??= GetComponentInChildren<AnchorPositionPunchReminderAnimation>(true);
            _scalePunchReminder ??= GetComponentInChildren<ScalePunchReminderAnimation>(true);
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
            Disable();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _nonRewardedQuestSubscription = _questsManager.NonRewardedQuests
                .ObserveCountChanged()
                .DoOnSubscribe(UpdatePanelState)
                .Subscribe(_ => UpdatePanelState());
        }

        private void StopObserving()
        {
            _nonRewardedQuestSubscription?.Dispose();
        }

        private void UpdatePanelState()
        {
            int nonRewardedQuestsCount = _questsManager.NonRewardedQuests.Count;

            if (nonRewardedQuestsCount > 0)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            Enable();
            _showAnimation.Play(() =>
            {
                _positionPunchReminder.Play();
                _scalePunchReminder.Play();
            });
        }

        private void Hide()
        {
            _hideAnimation.Play(Disable);
            _positionPunchReminder.Stop();
            _scalePunchReminder.Stop();
        }

        private void Enable()
        {
            _quest.SetActive(true);
        }

        private void Disable()
        {
            _quest.SetActive(false);
        }

        private void OnRestart()
        {
            Disable();
        }
    }
}