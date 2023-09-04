using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.GridCells.Logic
{
    public class GridCellTargetCardCollector : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private GridCellCardData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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

            _cardData.ContainsTargetCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.BottomCard.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentChanged()
        {
            CardData bottomCard = _cardData.BottomCard.Value;

            if (bottomCard == null) return;

            if (_cardData.ContainsTargetCard.Value)
            {
                bottomCard.UnlinkFromUpper();

                return;
            }

            if (bottomCard.Card.Value != _cardData.TargetCard.Value) return;

            bottomCard.gameObject.SetActive(false);
            
            _cardData.ContainsTargetCard.Value = true;
        }
    }
}
