using Quests.Data;
using Quests.Logic.Tutorials.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Logic
{
    public class CurrentQuestTutorialExecutor : MonoBehaviour
    {
        private CompositeDisposable _questsSubscriptions = new CompositeDisposable();

        private QuestTutorialExecutor _previousTutorialExecutor;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopTutorial();
        }

        #endregion

        private void StartObserving()
        {
            _questsManager.CurrentQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
        }

        private void StopObserving()
        {
            _questsSubscriptions.Clear();
        }

        private void OnQuestsUpdated()
        {
            if (_questsManager.CurrentQuest.Value != null && _questsManager.CurrentNonRewardedQuest.Value == null)
            {
                StartTutorial();
            }
            else
            {
                StopTutorial();
            }
        }

        private void StartTutorial()
        {
            StopTutorial();

            QuestData currentQuest = _questsManager.CurrentQuest.Value;

            if (currentQuest == null) return;

            QuestTutorialExecutor tutorialExecutor = _questsManager.CurrentQuest.Value.TutorialExecutor;

            if (tutorialExecutor == null) return;

            tutorialExecutor.StartTutorial();

            _previousTutorialExecutor = tutorialExecutor;
        }

        private void StopTutorial()
        {
            if (_previousTutorialExecutor != null)
            {
                _previousTutorialExecutor.StopTutorial();
            }
        }
    }
}
