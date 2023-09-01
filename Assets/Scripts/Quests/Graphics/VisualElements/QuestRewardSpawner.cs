﻿using System;
using System.Linq;
using Cards.Core;
using Cards.Logic.Spawn;
using Constraints.CardTable;
using Data.Player.Core;
using Data.Player.Experience;
using Graphics.Animations.Quests.QuestPanel;
using Graphics.UI.Particles.Coins.Logic;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestRewardSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        [Header("Particle Preferences")]
        [SerializeField] private ParticleSystem _particleSystem;

        private IDisposable _clickSubscription;
        private IDisposable _questShowAnimationSubscription;

        private QuestsManager _questsManager;
        private CoinsCollector _coinsCollector;
        private CardSpawner _cardSpawner;
        private ExperienceData _experienceData;
        private QuestShowAnimation _questShowAnimation;
        private PlayingAreaTableBounds _playingAreaTableBounds;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            CoinsCollector coinsCollector,
            CardSpawner cardSpawner,
            PlayerData playerData,
            QuestShowAnimation questShowAnimation,
            PlayingAreaTableBounds playingAreaTableBounds)
        {
            _questsManager = questsManager;
            _coinsCollector = coinsCollector;
            _cardSpawner = cardSpawner;
            _experienceData = playerData.ExperienceData;
            _questShowAnimation = questShowAnimation;
            _playingAreaTableBounds = playingAreaTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            StartObservingClick();
        }

        private void OnDisable()
        {
            StopObservingClick();

            _questShowAnimationSubscription?.Dispose();
        }

        #endregion

        private void StartObservingClick()
        {
            StopObservingClick();
            _clickSubscription = _button.OnClickAsObservable().Subscribe(_ => OnClick());
        }

        private void StopObservingClick()
        {
            _clickSubscription?.Dispose();
        }

        private void OnClick()
        {
            QuestData targetQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (targetQuest == null) return;

            _questShowAnimationSubscription?.Dispose();
            _questShowAnimationSubscription = Observable
                .FromEvent(
                    h => _questShowAnimation.OnPlay += h,
                    h => _questShowAnimation.OnPlay -= h)
                .Subscribe(_ => SpawnReward(targetQuest));

            MarkAsTookReward(targetQuest);
        }

        private void SpawnReward(QuestData questData)
        {
            SpawnCardReward(questData);

            GainExperience(questData);

            PlayParticle();

            MarkAsTookReward(questData);

            _questShowAnimationSubscription?.Dispose();
        }

        private void SpawnCardReward(QuestData questData)
        {
            int coinsCount = questData.Reward.Cards.Count(x => x == Card.Coin);

            Vector3 coinSpawnPosition;

            if (questData.ResultedCard != null)
            {
                coinSpawnPosition = questData.ResultedCard.transform.position;
            }
            else
            {
                coinSpawnPosition = _playingAreaTableBounds.transform.position;
            }

            Vector3 cardSpawnPosition = _playingAreaTableBounds.transform.position;

            _coinsCollector.Collect(coinsCount, coinSpawnPosition);

            foreach (var cardToSpawn in questData.Reward.Cards)
            {
                if (cardToSpawn != Card.Coin)
                {
                    _cardSpawner.SpawnAndMove(cardToSpawn, cardSpawnPosition);
                }
            }
        }

        private void MarkAsTookReward(QuestData questData)
        {
            questData.TookReward.Value = true;
        }

        private void PlayParticle()
        {
            _particleSystem.gameObject.SetActive(true);
            _particleSystem.Play();
        }

        private void GainExperience(QuestData questData)
        {
            if (questData.LevelUpAsReward)
            {
                _experienceData.TotalExperience.Value += _experienceData.ExperienceToNextLevel.Value;
            }
            else
            {
                _experienceData.TotalExperience.Value += questData.Reward.Experience;
            }
        }
    }
}
