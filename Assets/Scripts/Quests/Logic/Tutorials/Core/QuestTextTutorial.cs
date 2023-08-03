using System;
using UnityEngine;

namespace Quests.Logic.Tutorials.Core
{
    public class QuestTextTutorial : QuestTutorial
    {
        [Header("Preferences")]
        [TextArea, SerializeField] private string _tutorialText;

        public override void StartTutorial()
        {
            StopTutorial();
            ShowText();
            _isActive.Value = true;
        }

        public override void StopTutorial()
        {
            HideText();
            _isActive.Value = false;
        }

        protected void ShowText()
        {
            _tutorialTextPanel.Show();
            _tutorialTextPanel.Text = _tutorialText;
        }

        protected void HideText()
        {
            _tutorialTextPanel.Hide();
            _tutorialTextPanel.Text = String.Empty;
        }

        protected void ShowText(string text)
        {
            _tutorialTextPanel.Show();
            _tutorialTextPanel.Text = text;
            _tutorialText = text;
        }
    }
}
