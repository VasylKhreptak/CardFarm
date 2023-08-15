using System;
using Cards.Core;
using Cards.Logic.Spawn;
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

        [Header("Particle Preferences")]
        [SerializeField] private ParticleSystem _particleSystem;

        private IDisposable _clickSubscription;

        private QuestsManager _questsManager;
        private CoinsCollector _coinsCollector;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            CoinsCollector coinsCollector,
            CardSpawner cardSpawner)
        {
            _questsManager = questsManager;
            _coinsCollector = coinsCollector;
            _cardSpawner = cardSpawner;
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

            MarkAsTookReward(targetQuest);
        }

        private void SpawnReward(QuestData questData)
        {
            foreach (var cardToSpawn in questData.Reward.Cards)
            {
                if (cardToSpawn == Card.Coin)
                {
                    _coinsCollector.Collect(1, _button.transform.position);
                    continue;
                }

                _cardSpawner.SpawnAndMove(cardToSpawn, Vector3.zero);
            }
        }

        private void MarkAsTookReward(QuestData questData)
        {
            questData.TookReward.Value = true;
        }

        private void PlayParticle()
        {
            _particleSystem.Play();
        }
    }
}
