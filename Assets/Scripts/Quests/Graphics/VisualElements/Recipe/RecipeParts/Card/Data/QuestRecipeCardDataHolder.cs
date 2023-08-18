using Graphics.Animations;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements.Recipe.RecipeParts.Card.Data
{
    public class QuestRecipeCardDataHolder : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ScalePunchAnimation _scalePunchAnimation;

        public RectTransform RectTransform => _rectTransform;
        public ScalePunchAnimation ScalePunchAnimation => _scalePunchAnimation;

        public ColorReactiveProperty BackgroundColor = new ColorReactiveProperty();
        public ReactiveProperty<Sprite> Icon = new ReactiveProperty<Sprite>();
        public IntReactiveProperty Quantity = new IntReactiveProperty();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _rectTransform = GetComponent<RectTransform>();
            _scalePunchAnimation ??= GetComponentInChildren<ScalePunchAnimation>();
        }

        #endregion
    }
}
