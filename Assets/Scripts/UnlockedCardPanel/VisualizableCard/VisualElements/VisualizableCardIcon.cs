using Data.Cards.Core;
using UnityEngine;
using UnityEngine.UI;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardIcon : VisualizableCardObserver
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
            Sprite sprite = null;

            if (cardData != null)
            {
                sprite = cardData.Icon;
            }

            _image.sprite = sprite;
        }
    }
}
