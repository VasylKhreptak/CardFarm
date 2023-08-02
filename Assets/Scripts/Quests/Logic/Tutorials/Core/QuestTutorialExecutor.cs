using Graphics.UI.Panels;
using Graphics.UI.VisualElements;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public abstract class QuestTutorialExecutor : MonoBehaviour
    {
        protected TutorialHand _tutorialHand;
        protected TutorialTextPanel _tutorialTextPanel;

        [Inject]
        private void Constructor(TutorialHand tutorialHand, TutorialTextPanel tutorialTextPanel)
        {
            _tutorialHand = tutorialHand;
            _tutorialTextPanel = tutorialTextPanel;
        }

        public abstract void StartTutorial();

        public abstract void StopTutorial();
    }
}
