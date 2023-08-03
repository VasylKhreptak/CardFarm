using Graphics.UI.Panels;
using Graphics.UI.VisualElements;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public abstract class QuestTutorial : MonoBehaviour
    {
        protected BoolReactiveProperty _isActive = new BoolReactiveProperty();
        protected BoolReactiveProperty _isFinished = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;
        public IReadOnlyReactiveProperty<bool> IsFinished => _isFinished;

        protected TutorialHand _tutorialHand;
        protected TutorialTextPanel _tutorialTextPanel;

        [Inject]
        private void Constructor(TutorialHand tutorialHand, TutorialTextPanel tutorialTextPanel)
        {
            _tutorialHand = tutorialHand;
            _tutorialTextPanel = tutorialTextPanel;
        }

        [Button()]
        public virtual void StartTutorial()
        {
            StopTutorial();
            
            ResetFinishedState();
            _isActive.Value = true;
        }

        [Button()]
        public virtual void StopTutorial()
        {
            _isActive.Value = false;
        }

        public virtual void ResetFinishedState()
        {
            _isFinished.Value = false;
        }
    }
}
