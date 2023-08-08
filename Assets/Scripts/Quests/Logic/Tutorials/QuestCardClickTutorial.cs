using System;
using Cards.Data;
using Providers.Graphics;
using Quests.Logic.Tutorials.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials
{
    public class QuestCardClickTutorial : QuestCardInteractionTutorial
    {
        [Header("Preferences")]
        [SerializeField] private float _handClickInterval;
        [SerializeField] private Vector3 _handOffset;

        private IDisposable _handPositionSubscription;
        private IDisposable _handClickSubscription;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            _handPositionSubscription?.Dispose();
            _handClickSubscription?.Dispose();
            StopObservingCardClick();
            _tutorialHand.Hide();
        }

        protected override void OnFoundCard(CardDataHolder cardData)
        {
            base.OnFoundCard(cardData);

            _tutorialHand.Show();

            _handPositionSubscription?.Dispose();
            _handPositionSubscription = Observable
                .EveryUpdate()
                .DoOnSubscribe(UpdateHandPosition)
                .Subscribe(_ => UpdateHandPosition());

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

        protected virtual void OnCardClicked()
        {
            StopTutorial();
            _isFinished.Value = true;
        }
    }
}
