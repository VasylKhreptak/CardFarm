using System;
using Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Logic.Updaters
{
    public class QuestRecipeCardIconQuantityUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private QuestRecipeCardDataHolder _cardData;
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "x{0}";

        private IDisposable _subscription;

        #region MonoBehaivour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<QuestRecipeCardDataHolder>(true);
            _tmp ??= GetComponent<TMP_Text>();
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
            _subscription = _cardData.Quantity.Subscribe(UpdateQuantity);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateQuantity(int quantity)
        {
            _tmp.text = string.Format(_format, quantity);
            _tmp.enabled = quantity > 1;
        }
    }
}
