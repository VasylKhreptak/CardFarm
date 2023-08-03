using System.Collections.Generic;
using System.Linq;
using Quests.Logic.Tutorials.Core;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials
{
    public class QuestStepsTutorial : QuestTextTutorial, IValidatable
    {
        [Header("References")]
        [SerializeField] private List<QuestTutorial> _steps = new List<QuestTutorial>();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _steps = transform.GetComponentsInChildren<QuestTutorial>().ToList();
            _steps.Remove(this);
        }

        #endregion
    }
}
