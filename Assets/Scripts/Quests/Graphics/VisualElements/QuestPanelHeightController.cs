using System;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestPanelHeightController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Preferences")]
        [SerializeField] private float _recipeHeight;
        [SerializeField] private float _noRecipeHeight;

        private IDisposable _subscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        private void Start()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        private void StartObserving()
        {
            _subscription = _questsManager.CurrentQuest.Subscribe(UpdateHeight);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateHeight(QuestData questData)
        {
            if (questData == null) return;

            if (questData.Recipe.IsValid())
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _recipeHeight);
            }
            else
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _noRecipeHeight);
            }
        }
    }
}
