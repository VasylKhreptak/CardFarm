using System.Collections.Generic;
using System.Linq;
using Quests.Logic.Tutorials.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials
{
    public class QuestStepsTutorial : QuestTutorial, IValidatable
    {
        [Header("References")]
        [SerializeField] private List<QuestTutorial> _tutorials = new List<QuestTutorial>();

        private CompositeDisposable _finishedTutorialSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tutorials = transform.GetComponentsInChildren<QuestTutorial>().ToList();
            _tutorials.Remove(this);
        }

        #endregion

        public override void StartTutorial()
        {
            base.StartTutorial();

            StartObservingTutorials();
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            StopObservingTutorials();

            foreach (var tutorial in _tutorials)
            {
                tutorial.StopTutorial();
            }
        }

        private void StartObservingTutorials()
        {
            StopObservingTutorials();

            foreach (var tutorial in _tutorials)
            {
                tutorial.IsFinished.Subscribe(_ => OnTutorialFinishedStateUpdated(tutorial)).AddTo(_finishedTutorialSubscriptions);
            }
        }

        private void StopObservingTutorials()
        {
            _finishedTutorialSubscriptions?.Clear();
        }

        public override void ResetFinishedState()
        {
            base.ResetFinishedState();

            foreach (var tutorial in _tutorials)
            {
                tutorial.ResetFinishedState();
            }
        }

        private void OnTutorialFinishedStateUpdated(QuestTutorial tutorial)
        {
            if (_tutorials.All(x => x.IsFinished.Value))
            {
                StopTutorial();
                _isFinished.Value = true;
            }
            
            UpdateCurrentTutorial();
        }

        private void UpdateCurrentTutorial()
        {
            QuestTutorial nonFinishedTutorial = _tutorials.FirstOrDefault(x => x.IsFinished.Value == false);

            if (nonFinishedTutorial == null) return;

            nonFinishedTutorial.StartTutorial();
        }
    }
}
