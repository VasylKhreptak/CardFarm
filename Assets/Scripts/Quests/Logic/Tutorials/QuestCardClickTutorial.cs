using System;
using Cards.Data;
using Extensions;
using Providers.Graphics.UI;
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

        private Canvas _canvas;
        private RectTransform _canvasRectTransform;

        [Inject]
        private void Constructor(CanvasProvider canvasProvider)
        {
            _canvas = canvasProvider.Value;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            _handPositionSubscription?.Dispose();
            _handClickSubscription?.Dispose();
            StopObservingCardClick();
            _tutorialHand.Hide();
        }

        protected override void OnFoundCard(CardData cardData)
        {
            base.OnFoundCard(cardData);

            _tutorialHand.Show();

            _handPositionSubscription?.Dispose();
            _handPositionSubscription = Observable
                .EveryGameObjectUpdate()
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
            Vector3 targetScreenPosition = ConvertPoint(cardPosition + _handOffset);
            
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

        private Vector3 ConvertPoint(Vector3 point)
        {
            return RectTransformUtilityExtensions.ProjectPointOnCameraCanvas(_canvas, _canvasRectTransform, point);
        }
    }
}
