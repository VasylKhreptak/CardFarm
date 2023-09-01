using CBA.Animations.Sequences.Core;
using Graphics.Animations.Quests.QuestPanel;
using Runtime.Days;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Panels
{
    public class DaysProgressPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;

        [Header("Preferences")]
        [SerializeField] private AnimationSequence _showAnimation;

        private DaysRunner _daysRunner;
        private QuestShowAnimation _questShowAnimation;

        [Inject]
        private void Constructor(DaysRunner daysRunner,
            QuestShowAnimation questShowAnimation)
        {
            _daysRunner = daysRunner;
            _questShowAnimation = questShowAnimation;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _showAnimation.Stop();
            _panelObject.SetActive(false);
            _daysRunner.StopRunningDays();
            _questShowAnimation.OnCompleted -= OnCompletedShowAnimation;
            _questShowAnimation.OnCompleted += OnCompletedShowAnimation;

            _questShowAnimation.OnCompleted += OnCompletedShowAnimation;
        }

        private void OnDestroy()
        {
            _questShowAnimation.OnCompleted -= OnCompletedShowAnimation;
        }

        #endregion

        private void OnCompletedShowAnimation()
        {
            _panelObject.SetActive(true);
            _showAnimation.Stop();
            _showAnimation.PlayFromStartImmediate();
            _daysRunner.StartRunningDays();
            _questShowAnimation.OnCompleted -= OnCompletedShowAnimation;
        }
    }
}
