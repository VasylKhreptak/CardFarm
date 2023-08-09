using Cards.Data;
using CardsTable.ManualCardSelectors;
using Data.Cards.Core;
using ScriptableObjects.Scripts.Cards.Data;
using UnityEngine;
using UnityEngine.UI;
using UnlockedCardPanel.Graphics.Animations;
using UnlockedCardPanel.VisualizableCard.Data;
using Zenject;

namespace UnlockedCardPanel.Graphics.VisualElements
{
    public class NewCardPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;
        [SerializeField] private Button _closeButton;
        [SerializeField] private VisualizableCardData _cardVisualizerData;

        [Header("Preferences")]
        [SerializeField] private NewCardPanelShowAnimation _showAnimation;
        [SerializeField] private NewCardPanelHideAnimation _hideAnimation;

        private InvestigatedCardsObserver _investigatedCardsObserver;
        private CardsData _cardsData;

        [Inject]
        private void Constructor(InvestigatedCardsObserver investigatedCardsObserver, CardsData cardsData)
        {
            _investigatedCardsObserver = investigatedCardsObserver;
            _cardsData = cardsData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _closeButton ??= GetComponentInChildren<Button>();
            _cardVisualizerData ??= GetComponentInChildren<VisualizableCardData>();
            _closeButton ??= GetComponentInParent<Button>();
        }

        private void Awake()
        {
            Disable();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Hide);
            _investigatedCardsObserver.OnInvestigatedCard += OnInvestigatedNewCard;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Hide);
            _investigatedCardsObserver.OnInvestigatedCard -= OnInvestigatedNewCard;
        }

        #endregion

        private void Show()
        {
            _hideAnimation.Stop();
            Enable();
            _showAnimation.Play();
        }

        private void Hide()
        {
            _showAnimation.Stop();
            _hideAnimation.Play(Disable);
        }

        private void Enable() => _panelObject.SetActive(true);

        private void Disable() => _panelObject.SetActive(false);

        private void OnInvestigatedNewCard(CardData cardData)
        {
            if (_cardsData.TryGetValue(cardData.Card.Value, out CardDataHolder cardDataHolder))
            {
                _cardVisualizerData.VisualizableCard.Value = cardDataHolder;
            }

            Show();
        }
    }
}
