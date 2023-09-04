using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.GridCells.Logic
{
    public class CraftingGridCellAlphaController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private GridCellCardData _cardData;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<GridCellCardData>(true);
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
            StopObserving();

            _subscription = _cardData.ContainsTargetCard.Subscribe(OnContainsTargetCardChanged);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnContainsTargetCardChanged(bool containsTargetCard)
        {
            _cardData.CanvasGroup.alpha = containsTargetCard ? 1f : 0.5f;
        }
    }
}
