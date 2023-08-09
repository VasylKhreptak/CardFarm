using Data.Cards.Core;
using TMPro;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardNameText : VisualizableCardObserver
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
            string name;

            name = cardData == null ? string.Empty : cardData.Name;

            _tmp.text = name;
        }
    }
}
