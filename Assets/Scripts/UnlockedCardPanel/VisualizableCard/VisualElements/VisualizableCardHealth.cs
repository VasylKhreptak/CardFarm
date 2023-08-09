using Data.Cards;
using Data.Cards.Core;
using TMPro;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.VisualElements.Core;

namespace UnlockedCardPanel.VisualizableCard.VisualElements
{
    public class VisualizableCardHealth : VisualizableCardObserver
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
            if (cardData is DamageableCardDataHolder damageableCardData)
            {
                _graphicsObject.SetActive(true);

                _tmp.text = damageableCardData.Health.ToString();
                _tmp.color = damageableCardData.StatsTextColor;
            }
            else
            {
                _graphicsObject.SetActive(false);
            }
        }
    }
}
