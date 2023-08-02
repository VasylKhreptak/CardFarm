using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestPanelHeightController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _recipeHeight;
        [SerializeField] private float _noRecipeHeight;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        private void Start()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            _questsManager.CurrentQuest.Subscribe(_ => UpdateHeight()).AddTo(_subscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => UpdateHeight()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void UpdateHeight()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            QuestData nonRewardedQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (currentQuest == null || nonRewardedQuest != null) return;

            if (currentQuest.Recipe.IsValid())
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _recipeHeight);
            }
            else
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _noRecipeHeight);
            }
        }
    }
}
