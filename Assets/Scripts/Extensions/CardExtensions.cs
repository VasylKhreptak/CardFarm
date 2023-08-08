using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Extensions
{
    public static class CardExtensions
    {
        public static void LinkTo(this CardDataHolder card, CardDataHolder linkTo)
        {
            if (card == null || linkTo == null) return;

            if (card == linkTo) return;

            if (card.gameObject.activeSelf == false) return;

            CardDataHolder upperCard = card.UpperCard.Value;

            if (upperCard != null && upperCard != linkTo)
            {
                upperCard.UnlinkFromBottom();
            }

            CardDataHolder linkToBottomCard = linkTo.BottomCard.Value;

            if (linkToBottomCard != null && linkToBottomCard != card)
            {
                linkTo.UnlinkFromBottom();
            }

            card.UpperCard.Value = linkTo;

            linkTo.BottomCard.Value = card;
        }

        public static void UnlinkFromUpper(this CardDataHolder card)
        {
            if (card == null) return;

            CardDataHolder upperCard = card.UpperCard.Value;

            card.UpperCard.Value = null;

            if (upperCard == null) return;

            upperCard.BottomCard.Value = null;
        }

        public static void UnlinkFromBottom(this CardDataHolder card)
        {
            if (card == null) return;

            CardDataHolder bottomCard = card.BottomCard.Value;

            bottomCard.UnlinkFromUpper();
        }

        public static void Separate(this CardDataHolder card)
        {
            card.UnlinkFromUpper();
            card.UnlinkFromBottom();
        }

        public static List<CardDataHolder> FindBottomCards(this CardDataHolder card)
        {
            List<CardDataHolder> bottomCards = new List<CardDataHolder>();

            CardDataHolder currentCardData = card;

            while (currentCardData.BottomCard.Value != null)
            {
                CardDataHolder targetCard = currentCardData.BottomCard.Value;

                if (bottomCards.Count > 0 && bottomCards[0] == targetCard) break;

                bottomCards.Add(targetCard);
                currentCardData = targetCard;
            }

            return bottomCards;
        }

        public static List<CardDataHolder> FindUpperCards(this CardDataHolder card)
        {
            List<CardDataHolder> upperCards = new List<CardDataHolder>();

            CardDataHolder currentCardData = card;

            while (currentCardData.UpperCard.Value != null)
            {
                CardDataHolder targetCard = currentCardData.UpperCard.Value;

                if (upperCards.Count > 0 && upperCards[0] == targetCard) break;

                upperCards.Add(targetCard);
                currentCardData = targetCard;
            }

            upperCards.Reverse();

            return upperCards;
        }

        public static List<CardDataHolder> FindGroupCards(this CardDataHolder card)
        {
            List<CardDataHolder> UpperCards = card.FindUpperCards();
            List<CardDataHolder> BottomCards = card.FindBottomCards();

            List<CardDataHolder> GroupCards = new List<CardDataHolder>(UpperCards.Count + 1 + BottomCards.Count);

            GroupCards.AddRange(UpperCards);
            GroupCards.Add(card);
            GroupCards.AddRange(BottomCards);

            return GroupCards;
        }

        public static void RenderOnTop(this CardDataHolder card)
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
