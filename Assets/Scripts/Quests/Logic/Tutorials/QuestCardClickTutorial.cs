using System;
using Cards.Core;
using Cards.Data;
using CardsTable.Core;
using Providers.Graphics;
using Quests.Logic.Tutorials.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials
{
    public class QuestCardClickTutorial : QuestTutorialExecutor
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetCard;
        [SerializeField] private float _handClickInterval;
        [SerializeField] private Vector3 _handOffset;
        [TextArea, SerializeField] private string _tutorialText;

        private CompositeDisposable _cardTableSubscriptions = new CompositeDisposable();

        private IDisposable _handPositionSubscription;
        private IDisposable _handClickSubscription;

        private CardData _foundCard;

        private Camera _camera;
        private CardSelector _cardSelector;

        [Inject]
        private void Constructor(CameraProvider cameraProvider,
            CardSelector cardSelector)
        {
            _camera = cameraProvider.Value;
            _cardSelector = cardSelector;
        }

        public override void StartTutorial()
        {
            StopTutorial();
            StartObservingCardTable();
            ShowText();
        }

        public override void StopTutorial()
        {
            StopObservingCardTable();
            _tutorialHand.Hide();
            _foundCard = null;
            _handPositionSubscription?.Dispose();
            _handClickSubscription?.Dispose();
            _tutorialHand.Hide();
            StopObservingCardClick();
            HideText();
        }

        private void StartObservingCardTable()
        {
            StopObservingCardTable();

            if (_cardSelector.SelectedCardsMap.TryGetValue(_targetCard, out var cards))
            {
                OnFoundCard(cards[0]);
                StopObservingCardTable();
                return;
            }

            _cardSelector.SelectedCardsMap.ObserveAdd().Subscribe(addEvent =>
            {
                if (addEvent.Key == _targetCard)
                {
                    OnFoundCard(addEvent.Value[0]);
                    StopObservingCardTable();
                }
            }).AddTo(_cardTableSubscriptions);
        }

        private void StopObservingCardTable()
        {
            _cardTableSubscriptions.Clear();
        }

        private void OnFoundCard(CardData cardData)
        {
            _tutorialHand.Show();

            _foundCard = cardData;

            _handPositionSubscription?.Dispose();
            _handPositionSubscription = Observable.EveryUpdate().Subscribe(_ => UpdateHandPosition());

            _handClickSubscription?.Dispose();
            _handClickSubscription = Observable.Interval(TimeSpan.FromSeconds(_handClickInterval)).Subscribe(_ =>
            {
                _tutorialHand.Click();
            });

            StartObservingCardClick();
        }

        private void UpdateHandPosition()
        {
            if (_foundCard == null) return;

            Vector3 cardPosition = _foundCard.transform.position;
            Vector3 targetScreenPosition = _camera.WorldToScreenPoint(cardPosition + _handOffset);

            _tutorialHand.SetPosition(targetScreenPosition);
        }

        private void StartObservingCardClick()
        {
            StopObservingCardClick();

            _foundCard.Callbacks.onClicked += OnCardClicked;
        }

        private void StopObservingCardClick()
        {
            if (_foundCard == null) return;

            _foundCard.Callbacks.onClicked -= OnCardClicked;
        }

        private void OnCardClicked()
        {
            StopTutorial();
        }

        private void ShowText()
        {
            _tutorialTextPanel.Show();
            _tutorialTextPanel.Text = _tutorialText;
        }

        private void HideText()
        {
            _tutorialTextPanel.Hide();
            _tutorialTextPanel.Text = String.Empty;
        }
    }
}
