using Graphics.UI.Panels;
using Graphics.UI.VisualElements;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public abstract class QuestTutorial : MonoBehaviour
    {
        protected BoolReactiveProperty _isActive = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

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
