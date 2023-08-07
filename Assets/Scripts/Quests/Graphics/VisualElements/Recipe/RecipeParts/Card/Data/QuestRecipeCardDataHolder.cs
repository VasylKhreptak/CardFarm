using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data
{
    public class QuestRecipeCardDataHolder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;

        public ColorReactiveProperty BackgroundColor = new ColorReactiveProperty();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
        public IntReactiveProperty Quantity = new IntReactiveProperty();

        public void CopyFrom(QuestRecipeCardData data)
        {
            BackgroundColor.Value = data.BackgroundColor;
            Icon.Value = data.Icon;
            Quantity.Value = data.Quantity;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        #endregion
    }
}
