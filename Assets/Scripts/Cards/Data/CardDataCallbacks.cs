﻿using System;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Recipes;

namespace Cards.Data
{
    public class CardDataCallbacks
    {
        public Action onAnyBottomCardUpdated;
        public Action onBottomCardsListUpdated;

        public Action onAnyUpperCardUpdated;
        public Action onUpperCardsListUpdated;

        public Action onGroupCardsListUpdated;
        public Action onBecameHeadOfGroup;

        public Action<Card> OnReproduced;
        public Action OnReproducedNoArgs;

        public Action<CardRecipe> onExecutedRecipe;

        public Action onClicked;
    }
}
