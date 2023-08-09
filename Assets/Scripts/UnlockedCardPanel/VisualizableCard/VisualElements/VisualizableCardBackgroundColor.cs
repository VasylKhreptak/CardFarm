using Data.Cards.Core;
using UnityEngine;
using UnityEngine.UI;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardBackgroundColor : VisualizableCardObserver
    {
        [Header("References")]
        [SerializeField] private Image _image;

        public override void Validate()
        {
            base.Validate();

            _image ??= GetComponent<Image>();
        }

        protected override void OnCardDataChanged(CardDataHolder cardData)
        {
            Color color = Color.white;

            if (cardData != null)
            {
                color = cardData.BodyColor;
            }

            _image.color = color;
        }
    }
}
