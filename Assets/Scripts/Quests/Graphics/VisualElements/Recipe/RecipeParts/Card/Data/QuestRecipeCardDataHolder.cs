using UniRx;
using UnityEngine;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data
{
    public class QuestRecipeCardDataHolder : MonoBehaviour
    {
        public ColorReactiveProperty BackgroundColor = new ColorReactiveProperty();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
        public IntReactiveProperty Quantity = new IntReactiveProperty();
        
        public void CopyFrom(QuestRecipeCardData data)
        {
            BackgroundColor.Value = data.BackgroundColor;
            Icon.Value = data.Icon;
            Quantity.Value = data.Quantity;
        }
    }
}
