using System;
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

        private IDisposable _currentQuestSubscription;

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
            _currentQuestSubscription = _questsManager.CurrentQuest.Subscribe(OnCurrentQuestChanged);
        }

        private void StopObservingCurrentQuest()
        {
            _currentQuestSubscription?.Dispose();
        }

        private void OnCurrentQuestChanged(QuestData questData)
        {
            if (questData == null || questData.Reward.Cards.Length == 0)
            {
                _image.enabled = false;
                return;
            }

            if (questData.IsCompleted.Value == false) return;

            _image.enabled = true;
            _image.sprite = _cardsGraphicData.GetValue(questData.Reward.Cards[0]).Icon;
        }
    }
}
