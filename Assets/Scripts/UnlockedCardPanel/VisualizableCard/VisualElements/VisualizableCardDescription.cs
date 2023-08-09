using Data.Cards.Core;
using TMPro;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardDescription : VisualizableCardObserver
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
            string description = string.Empty;

            if (cardData != null)
            {
                description = cardData.Description;
            }

            _tmp.text = description;
        }
    }
}
