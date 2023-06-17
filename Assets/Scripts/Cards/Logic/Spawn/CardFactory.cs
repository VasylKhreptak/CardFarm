using Cards.Core;
using Cards.Data;
using Extensions;
using Factories.Core;
using ObjectPoolers;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Spawn
{
    public class CardFactory : MonoBehaviour, IParameterizedFactory<Card, CardData>
    {
        private CardsObjectPooler _cardsObjectPooler;

        [Inject]
        private void Constructor(CardsObjectPooler cardsObjectPooler)
        {
            _cardsObjectPooler = cardsObjectPooler;
        }

        public CardData Create(Card card)
        {
            GameObject cardObject = _cardsObjectPooler.Spawn(card);
            CardData cardData = cardObject.GetComponent<CardData>();
            cardData.RenderOnTop();
            return cardData;
        }
    }
}
