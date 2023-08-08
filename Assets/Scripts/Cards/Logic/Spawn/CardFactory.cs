using Cards.Core;
using Cards.Data;
using Extensions;
using Factories.Core;
using ObjectPoolers;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Spawn
{
    public class CardFactory : MonoBehaviour, IParameterizedFactory<Card, CardDataHolder>
    {
        private CardsObjectPooler _cardsObjectPooler;

        [Inject]
        private void Constructor(CardsObjectPooler cardsObjectPooler)
        {
            _cardsObjectPooler = cardsObjectPooler;
        }

        public CardDataHolder Create(Card card)
        {
            GameObject cardObject = _cardsObjectPooler.Spawn(card);
            CardDataHolder cardData = cardObject.GetComponent<CardDataHolder>();
            cardData.RenderOnTop();
            return cardData;
        }
    }
}
