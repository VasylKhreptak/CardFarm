using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Extensions
{
    public static class CardExtensions
    {
        public static void LinkTo(this CardData card, CardData linkTo)
        {
            if (card == null || linkTo == null) return;

            if (card == linkTo) return;

            card.UpperCard.Value = linkTo;
            linkTo.BottomCard.Value = card;
        }

        public static void UnlinkFromUpper(this CardData card)
        {
            if (card == null) return;

            CardData upperCard = card.UpperCard.Value;

            if (upperCard == null) return;

            upperCard.BottomCard.Value = null;

            card.UpperCard.Value = null;
        }

        public static void Separate(this CardData card)
        {
            if (card == null) return;

            card.UnlinkFromUpper();

            if (card.BottomCard.Value != null)
            {
                card.BottomCard.Value.UnlinkFromUpper();
            }
        }

        public static List<CardData> FindBottomCards(this CardData card)
        {
            List<CardData> bottomCards = new List<CardData>();

            CardData currentCardData = card;

            while (currentCardData.BottomCard.Value != null)
            {
                CardData targetCard = currentCardData.BottomCard.Value;

                if (bottomCards.Count > 0 && bottomCards[0] == targetCard) break;

                bottomCards.Add(targetCard);
                currentCardData = targetCard;
            }

            return bottomCards;
        }

        public static List<CardData> FindUpperCards(this CardData card)
        {
            List<CardData> upperCards = new List<CardData>();

            CardData currentCardData = card;

            while (currentCardData.UpperCard.Value != null)
            {
                CardData targetCard = currentCardData.UpperCard.Value;

                if (upperCards.Count > 0 && upperCards[0] == targetCard) break;

                upperCards.Add(targetCard);
                currentCardData = targetCard;
            }

            upperCards.Reverse();

            return upperCards;
        }

        public static List<CardData> FindGroupCards(this CardData card)
        {
            List<CardData> UpperCards = card.FindUpperCards();
            List<CardData> BottomCards = card.FindBottomCards();

            List<CardData> GroupCards = new List<CardData>(UpperCards.Count + 1 + BottomCards.Count);

            GroupCards.AddRange(UpperCards);
            GroupCards.Add(card);
            GroupCards.AddRange(BottomCards);

            return GroupCards;
        }

        public static void RenderOnTop(this CardData card)
        {
            if (card.CanSortingLayerChange == false) return;

            Transform parent = card.transform.parent;

            if (parent != null && parent.gameObject.activeInHierarchy)
            {
                card.transform.SetAsLastSibling();
            }
        }
    }
}
