using System;
using Cards.Core;
using Quests.Data;
using Quests.Logic;
using ScriptableObjects.Scripts.Cards.Graphics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class CurrentQuestRewardIcon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

        [Header("Preferences")]
        [SerializeField] private CardsGraphicData _cardsGraphicData;
        [SerializeField] private Sprite _defaultCardSprite;

        private IDisposable _questSubscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartObservingCurrentQuest();
        }

        private void OnDisable()
        {
            StopObservingCurrentQuest();
        }

        #endregion

        private void StartObservingCurrentQuest()
        {
            _questSubscription = _questsManager.CurrentNonRewardedQuest.Subscribe(OnCurrentNonRewardedQuestChanged);
        }

        private void StopObservingCurrentQuest()
        {
            _questSubscription?.Dispose();
        }

        private void OnCurrentNonRewardedQuestChanged(QuestData questData)
        {
            if (questData == null || questData.Reward.Cards.Length == 0)
            {
                _image.enabled = false;
                return;
            }

            if (questData.IsCompleted.Value == false) return;

            _image.enabled = true;

            Card reward = questData.Reward.Cards[0];

            if (reward != Card.Coin)
            {
                _image.sprite = _defaultCardSprite;
            }
            else
            {
                _image.sprite = _cardsGraphicData.GetValue(reward).Icon;
            }
        }
    }
}
