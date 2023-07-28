using System.Collections.Generic;
using Cards.Data;
using Constraints.CardTable;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CardPusher : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _pushSpeedAmplifier = 1f;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
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

        private void Update()
        {
            PushCardStep();
        }

        #endregion

        private void PushCardStep()
        {
            Vector3 position = _cardData.transform.position;
            Vector3 direction = Vector3.zero;
            List<CardData> overlappingCards = _cardData.OverlappingCards;

            for (int i = 0; i < overlappingCards.Count; i++)
            {
                CardData overlappingCard = overlappingCards[i];

                Vector3 directionToCard = overlappingCard.transform.position - _cardData.transform.position;

                directionToCard.Normalize();

                direction += directionToCard;
            }

            direction.y = 0;
            direction.Normalize();

            direction *= _pushSpeedAmplifier;

            position -= direction * Time.deltaTime;

            position = _cardsTableBounds.Clamp(_cardData.RectTransform, position);

            _cardData.transform.position = position;
        }
    }
}
