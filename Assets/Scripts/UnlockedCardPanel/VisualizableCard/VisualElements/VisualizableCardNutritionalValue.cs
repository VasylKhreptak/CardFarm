using Data.Cards;
using Data.Cards.Core;
using TMPro;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardNutritionalValue : VisualizableCardObserver
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] private GameObject _graphicsObject;

        private void Awake()
        {
            _graphicsObject.SetActive(false);
        }

        public override void Validate()
        {
            base.Validate();

            _tmp ??= GetComponent<TMP_Text>();
        }

        protected override void OnCardDataChanged(CardDataHolder cardData)
        {
            if (cardData is FoodCardDataHolder foodCardData)
            {
                _graphicsObject.SetActive(true);

                _tmp.text = foodCardData.NutritionalValue.ToString();
                _tmp.color = foodCardData.StatsTextColor;
            }
            else
            {
                _graphicsObject.SetActive(false);
            }
        }
    }
}
