using System;
using System.Linq;
using Cards.Core;
using Cards.Logic.Spawn;
using Data.Player.Core;
using Data.Player.Experience;
using Graphics.UI.Particles.Coins.Logic;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestRewardButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _rewardSpawnPlace;

        [Header("Particle Preferences")]
        [SerializeField] private ParticleSystem _particleSystem;

        private IDisposable _clickSubscription;

        private QuestsManager _questsManager;
        private CoinsCollector _coinsCollector;
        private CardSpawner _cardSpawner;
        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            CoinsCollector coinsCollector,
            CardSpawner cardSpawner,
            PlayerData playerData)
        {
            _questsManager = questsManager;
            _coinsCollector = coinsCollector;
            _cardSpawner = cardSpawner;
            _experienceData = playerData.ExperienceData;
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

            SpawnReward(targetQuest);

            GainExperience(targetQuest);

            MarkAsTookReward(targetQuest);

            PlayParticle();
        }

        private void SpawnReward(QuestData questData)
        {
            int coinsCount = questData.Reward.Cards.Count(x => x == Card.Coin);

            _coinsCollector.Collect(coinsCount, _rewardSpawnPlace.position);

            foreach (var cardToSpawn in questData.Reward.Cards)
            {
                if (cardToSpawn != Card.Coin)
                {
                    _cardSpawner.SpawnAndMove(cardToSpawn, Vector3.zero);
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
            _experienceData.TotalExperience.Value += questData.Reward.Experience;
        }
    }
}
