﻿using System.Collections.Generic;
using Cards.Data;

namespace Extensions.Cards
{
    public static class CardExtensions
    {
        public static void Link(this CardData card, CardData linkTo)
        {
            if (linkTo == null) return;

            CardData upperCard = card.UpperCard.Value;

            if (upperCard != null)
            {
                upperCard.BottomCard.Value = card;
            }

            card.UpperCard.Value = linkTo;
        }

        public static void Unlink(this CardData card)
        {
            CardData upperCard = card.UpperCard.Value;

            if (upperCard != null)
            {
                upperCard.BottomCard.Value = null;
            }

            card.UpperCard.Value = null;
        }

        public static List<CardData> FindBottomCards(this CardData card)
        {
            List<CardData> bottomCards = new List<CardData>();

            CardData currentCardData = card;

            while (currentCardData.BottomCard.Value != null)
            {
                bottomCards.Add(currentCardData.BottomCard.Value);
                currentCardData = currentCardData.BottomCard.Value;
            }

            return bottomCards;
        }

        public static List<CardData> FindUpperCards(this CardData card)
        {
            List<CardData> upperCards = new List<CardData>();

            CardData currentCardData = card;

            while (currentCardData.UpperCard.Value != null)
            {
                upperCards.Add(currentCardData.UpperCard.Value);
                currentCardData = currentCardData.UpperCard.Value;
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
    }
}
