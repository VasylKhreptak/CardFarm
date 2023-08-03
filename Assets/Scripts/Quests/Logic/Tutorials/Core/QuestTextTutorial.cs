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
            base.StartTutorial();
            
            ShowText();
        }

        public override void StopTutorial()
        {
            base.StopTutorial();
            
            HideText();
        }

        protected void ShowText()
        {
            _tutorialTextPanel.Text = _tutorialText;
            _tutorialTextPanel.Show();
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
