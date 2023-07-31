using Quests.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Logic
{
    public class QuestClickedEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _button;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(InvokeEvent);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(InvokeEvent);
        }

        #endregion

        private void InvokeEvent()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;

            if (currentQuest == null) return;

            currentQuest.Callbacks.OnCLicked?.Invoke();
        }
    }
}
