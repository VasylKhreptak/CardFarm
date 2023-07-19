using System;
using Cards.Core;
using Cards.Logic.Spawn;
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

        private IDisposable _clickSubscription;

        private QuestsManager _questsManager;
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(QuestsManager questsManager, CardSpawner cardSpawner)
        {
            _questsManager = questsManager;
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
            QuestData currentQuest = _questsManager.CurrentQuest.Value;

            if (currentQuest == null || currentQuest.TookReward.Value || currentQuest.IsCompleted.Value == false) return;

            SpawnReward();

            MarkAsTookReward();
        }

        private void SpawnReward()
        {
            foreach (var cardToSpawn in _questsManager.CurrentQuest.Value.Reward.Cards)
            {
                if (cardToSpawn == Card.Coin)
                {
                    _cardSpawner.SpawnCoinAndMove(Vector3.zero);
                    continue;
                }

                _cardSpawner.SpawnAndMove(cardToSpawn, Vector3.zero);
            }
        }

        private void MarkAsTookReward()
        {
            _questsManager.CurrentQuest.Value.TookReward.Value = true;
        }
    }
}
