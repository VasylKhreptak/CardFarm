﻿using System;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;

namespace Cards.AutomatedFactories.Data
{
    public class AutomatedCardFactoryData : SellableCardData
    {
        public ReactiveProperty<CardFactoryRecipe> CurrentFactoryRecipe = new ReactiveProperty<CardFactoryRecipe>();

        public AutomatedFactoryCallbacks AutomatedFactoryCallbacks = new AutomatedFactoryCallbacks();
    }
}
