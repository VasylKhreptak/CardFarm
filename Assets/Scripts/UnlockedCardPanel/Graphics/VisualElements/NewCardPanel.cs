using Cards.Data;
using CardsTable.ManualCardSelectors;
using Providers.Graphics;
using Runtime.Commands;
using ScriptableObjects.Scripts.Cards.Data;
using UniRx;
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
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GameObject _panelObject;
        [SerializeField] private Button _closeButton;
        [SerializeField] private VisualizableCardData _cardVisualizerData;

        [Header("Preferences")]
        [SerializeField] private float _flipDelay = 1f;
        [SerializeField] private NewCardPanelShowAnimation _showAnimation;
        [SerializeField] private NewCardPanelHideAnimation _hideAnimation;

        private BoolReactiveProperty _isActive = new BoolReactiveProperty(false);

        private CardData _investigatedCard;

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        private InvestigatedCardsObserver _investigatedCardsObserver;
        private CardsData _cardsData;
        private GameRestartCommand _gameRestartCommand;
        private Camera _camera;

        [Inject]
        private void Constructor(InvestigatedCardsObserver investigatedCardsObserver,
            CardsData cardsData,
            GameRestartCommand gameRestartCommand,
            CameraProvider cameraProvider)
        {
            _investigatedCardsObserver = investigatedCardsObserver;
            _cardsData = cardsData;
            _gameRestartCommand = gameRestartCommand;
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _closeButton ??= GetComponentInChildren<Button>();
            _cardVisualizerData ??= GetComponentInChildren<VisualizableCardData>();
            _closeButton ??= GetComponentInParent<Button>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void Awake()
        {
            Disable();

            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnClicked);
            _investigatedCardsObserver.OnInvestigatedCard += OnInvestigatedNewCard;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClicked);
            _investigatedCardsObserver.OnInvestigatedCard -= OnInvestigatedNewCard;
            StopObservingRaise(_investigatedCard);
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void Show()
        {
            _cardVisualizerData.FLipAnimation.Play(_flipDelay);

            _hideAnimation.Stop();
            Enable();

            _investigatedCard.OverlayDrawer.EnableOverlay();

            _showAnimation.Play(GetCardAnchoredPosition(), () =>
            {
                _investigatedCard.OverlayDrawer.DisableOverlay();
            });
        }

        private void Hide()
        {
            _showAnimation.Stop();

            if (_investigatedCard != null)
            {
                _investigatedCard.Animations.AppearAnimation.ShowCardSuit();
            }

            _investigatedCard.OverlayDrawer.DisableOverlay();

            _hideAnimation.Play(GetCardAnchoredPosition(), () =>
                {
                    Disable();

                    if (_investigatedCard != null)
                    {
                        _investigatedCard.IsInteractable.Value = false;
                        _investigatedCard.Animations.AppearAnimation.PlaceCardOnTable(() =>
                        {
                            _investigatedCard.IsInteractable.Value = true;
                        });
                    }

                    _investigatedCard.OverlayDrawer.DisableOverlay();
                },
                () =>
                {
                    _investigatedCard.OverlayDrawer.EnableOverlay();
                });
        }

        private void Enable()
        {
            _panelObject.SetActive(true);
            _isActive.Value = true;
        }

        private void Disable()
        {
            _panelObject.SetActive(false);
            _isActive.Value = false;
        }

        private void OnClicked()
        {
            if (_showAnimation.IsPlaying) return;

            Hide();
        }

        private void OnInvestigatedNewCard(CardData cardData)
        {
            _isActive.Value = true;

            _investigatedCard = cardData;

            _cardsData.TryGetValue(cardData.Card.Value, out var data);

            _cardVisualizerData.VisualizableCard.Value = data;

            StartObservingRaise(cardData);
        }

        private void OnRestart()
        {
            Disable();
        }

        private void StartObservingRaise(CardData card)
        {
            StopObservingRaise(_investigatedCard);
            StopObservingRaise(card);

            card.Animations.AppearAnimation.onRaised += OnCardRaised;
        }

        private void StopObservingRaise(CardData cardData)
        {
            if (cardData == null) return;

            cardData.Animations.AppearAnimation.onRaised -= OnCardRaised;
        }

        private void OnCardRaised()
        {
            Show();
            StopObservingRaise(_investigatedCard);
        }

        private Vector2 GetCardAnchoredPosition()
        {
            if (_investigatedCard == null) return Vector2.zero;

            Vector3 worldCardPosition = _investigatedCard.transform.position;
            Vector3 screenPoint = _camera.WorldToScreenPoint(worldCardPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPoint, _camera, out var localRectPoint);

            return localRectPoint;
        }
    }
}
