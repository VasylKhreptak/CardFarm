using System;
using Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Logic.Updaters
{
    public class QuestRecipeCardIconUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private QuestRecipeCardDataHolder _cardData;
        [SerializeField] private Image _image;

        private IDisposable _subscription;

        #region MonoBehaivour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<QuestRecipeCardDataHolder>(true);
            _image ??= GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _subscription = _cardData.Icon.Subscribe(UpdateIcon);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateIcon(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}
