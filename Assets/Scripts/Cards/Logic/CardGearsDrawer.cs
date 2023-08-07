using Cards.Data;
using CardsTable.PoolLogic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardGearsDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _height = 2f;

        private CompositeDisposable _cardDataSubscriptions = new CompositeDisposable();

        private CardTablePooler _cardTablePooler;

        [Inject]
        private void Constructor(CardTablePooler cardTablePooler)
        {
            _cardTablePooler = cardTablePooler;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        #endregion

        private void StartObservingCardData()
        {
            StopObservingCardData();
        }

        private void StopObservingCardData()
        {
            _cardDataSubscriptions.Clear();
        }

        private void OnCardDataUpdated()
        {

        }
    }
}
