using System;
using System.Collections.Generic;
using Cards.Data;
using CardsTable.ManualCardSelectors;
using Data.Cards.Core;
using Providers.Graphics;
using Runtime.Commands;
using ScriptableObjects.Scripts.Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnlockedCardPanel.Graphics.Animations;
using Zenject;

namespace UnlockedCardPanel.Graphics.VisualElements
{
    public class NewCardPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GameObject _panelObject;
        [SerializeField] private Button _closeButton;

        [Header("Preferences")]
        [SerializeField] private NewCardPanelShowAnimation _showAnimation;
        [SerializeField] private NewCardPanelHideAnimation _hideAnimation;

        [Header("Card Info")]
        [SerializeField] private List<GameObject> _cardInfoObjects;
        [SerializeField] private TMP_Text _descriptionText;

        private BoolReactiveProperty _isActive = new BoolReactiveProperty(false);

        private CardData _investigatedCard;
        private Vector3 _initialInvestigatedCardPosition;

        private IDisposable _delaySubscription;

        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        private InvestigatedCardsObserver _investigatedCardsObserver;
        private GameRestartCommand _gameRestartCommand;
        private CardsData _cardsData;
        private Camera _camera;

        [Inject]
        private void Constructor(InvestigatedCardsObserver investigatedCardsObserver,
            GameRestartCommand gameRestartCommand,
            CameraProvider cameraProvider,
            CardsData cardsData)
        {
            _investigatedCardsObserver = investigatedCardsObserver;
            _gameRestartCommand = gameRestartCommand;
            _camera = cameraProvider.Value;
            _cardsData = cardsData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _closeButton ??= GetComponentInChildren<Button>();
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
            // _investigatedCardsObserver.OnInvestigatedCard += OnInvestigatedNewCard;
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnClicked);
            // _investigatedCardsObserver.OnInvestigatedCard -= OnInvestigatedNewCard;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void Show(float delay = 0.7f, Action onPlay = null)
        {
            _isActive.Value = true;

            _delaySubscription?.Dispose();
            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                SetActiveCardInfoObjects(false);

                _hideAnimation.Stop();
                _panelObject.SetActive(false);
                Enable();

                onPlay?.Invoke();
                
                _showAnimation.Play(GetCardAnchoredPosition(), _investigatedCard, () =>
                {
                    SetActiveCardInfoObjects(true);

                    if (_cardsData.TryGetValue(_investigatedCard.Card.Value, out CardDataHolder cardDataHolder))
                    {
                        _descriptionText.text = cardDataHolder.Description;
                    }
                });
            });
        }

        private void Hide()
        {
            _showAnimation.Stop();

            _hideAnimation.Play(GetCardAnchoredPosition(), _investigatedCard, _initialInvestigatedCardPosition, Disable);
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
            if (_showAnimation.IsPlaying || _hideAnimation.IsPlaying) return;

            Hide();
        }

        private void OnInvestigatedNewCard(CardData cardData)
        {
            Show(cardData);
        }

        private void OnRestart()
        {
            Disable();
        }

        public void Show(CardData cardData, float delay = 0.7f, Action onStart = null)
        {
            _isActive.Value = true;

            _investigatedCard = cardData;
            _initialInvestigatedCardPosition = cardData.transform.position;

            Show(delay, onStart);
        }

        private Vector2 GetCardAnchoredPosition()
        {
            if (_investigatedCard == null) return Vector2.zero;

            Vector3 worldCardPosition = _investigatedCard.transform.position;
            Vector3 screenPoint = _camera.WorldToScreenPoint(worldCardPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPoint, _camera, out var localRectPoint);

            return localRectPoint;
        }

        private void SetActiveCardInfoObjects(bool isActive)
        {
            foreach (var cardInfoObject in _cardInfoObjects)
            {
                cardInfoObject.SetActive(isActive);
            }
        }
    }
}
