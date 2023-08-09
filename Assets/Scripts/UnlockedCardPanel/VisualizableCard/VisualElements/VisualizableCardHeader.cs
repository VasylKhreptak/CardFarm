using Data.Cards.Core;
using UnityEngine;
using UnityEngine.UI;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardHeader : VisualizableCardObserver
    {
        [Header("References")]
        [SerializeField] private Image _image;
        [SerializeField] private Image _headerUnderline;

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
                bool enabledHeader = cardData.HasHeaderBackground;
                _image.enabled = enabledHeader;
                _headerUnderline.enabled = enabledHeader;

                color = cardData.HeaderColor;
            }

            _image.color = color;
        }
    }
}
