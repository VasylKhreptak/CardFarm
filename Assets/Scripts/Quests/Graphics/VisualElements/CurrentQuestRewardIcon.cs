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

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            _questsManager.CurrentQuest.Subscribe(_ => OnQuestsDataUpdated()).AddTo(_subscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => OnQuestsDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObservingCurrentQuest()
        {
            _subscriptions?.Dispose();
        }

        private void OnQuestsDataUpdated()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            QuestData nonRewardedQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (currentQuest == null || nonRewardedQuest != null) return;

            _image.enabled = true;

            Card reward = currentQuest.Reward.Cards[0];

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
