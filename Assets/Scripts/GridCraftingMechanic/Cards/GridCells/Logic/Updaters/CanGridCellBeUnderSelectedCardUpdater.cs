using Cards.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.GridCells.Logic.Updaters
{
    public class CanGridCellBeUnderSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private GridCellCardData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CurrentSelectedCardHolder _selectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder selectedCardHolder)
        {
            _selectedCardHolder = selectedCardHolder;
        }

        #region MonoBehaivour

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

            _cardData.CanBeUnderSelectedCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _selectedCardHolder.SelectedCard.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentUpdated()
        {
            CardData selectedCard = _selectedCardHolder.SelectedCard.Value;

            bool canBeUnderSelectedCard = selectedCard != null && selectedCard.Card.Value == _cardData.TargetCard.Value;

            _cardData.CanBeUnderSelectedCard.Value = canBeUnderSelectedCard;
        }
    }
}
