using System;
using UnityEngine;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data
{
    [Serializable]
    public class QuestRecipeCardData
    {
        public Color BackgroundColor = Color.white;
        public Sprite Icon;
        public int Quantity = 1;
    }
}
