using System;
using UnityEngine;

namespace Quests.Logic.Tutorials.Core
{
    public class QuestTextTutorial : QuestTutorialExecutor
    {
        [Header("Preferences")]
        [TextArea, SerializeField] private string _tutorialText;

        public override void StartTutorial()
        {
            StopTutorial();
            ShowText();
        }

        public override void StopTutorial()
        {
            HideText();
        }

        private void ShowText()
        {
            _tutorialTextPanel.Show();
            _tutorialTextPanel.Text = _tutorialText;
        }

        private void HideText()
        {
            _tutorialTextPanel.Hide();
            _tutorialTextPanel.Text = String.Empty;
        }
    }
}
