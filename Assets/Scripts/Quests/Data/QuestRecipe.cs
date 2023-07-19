using System;
using System.Collections.Generic;
using Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data;

namespace Quests.Data
{
    [Serializable]
    public class QuestRecipe
    {
        public QuestRecipeCardData Result = new QuestRecipeCardData();
        public List<QuestRecipeCardData> TargetCards = new List<QuestRecipeCardData>();

        public bool IsValid()
        {
            return Result != null && TargetCards != null && TargetCards.Count > 0;
        }
    }
}
