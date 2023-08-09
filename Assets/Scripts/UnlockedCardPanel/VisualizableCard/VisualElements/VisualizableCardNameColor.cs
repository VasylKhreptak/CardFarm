using Data.Cards.Core;
using TMPro;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardNameColor : VisualizableCardObserver
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        public override void Validate()
        {
            base.Validate();

            _tmp ??= GetComponent<TMP_Text>();
        }

        protected override void OnCardDataChanged(CardDataHolder cardData)
        {
            Color color;

            color = cardData == null ? Color.white : cardData.NameColor;

            _tmp.color = color;
        }
    }
}
