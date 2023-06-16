﻿using System.Collections.Generic;
using Cards.Data;
using UnityEngine;

namespace Extensions
{
    public static class CardExtensions
    {
        public static void LinkTo(this CardData card, CardData linkTo)
        {
            if (card == null || linkTo == null) return;

            card.UpperCard.Value = linkTo;
            linkTo.BottomCard.Value = card;
        }

        public static void UnlinkFromUpper(this CardData card)
        {
            CardData upperCard = card.UpperCard.Value;

            if (upperCard == null) return;

            upperCard.BottomCard.Value = null;

            card.UpperCard.Value = null;
        }

        public static void Separate(this CardData card)
        {
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
                bottomCards.Add(currentCardData.BottomCard.Value);
                currentCardData = currentCardData.BottomCard.Value;

                if (bottomCards.Count > 1000)
                {
                    Debug.Break();
                }
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
