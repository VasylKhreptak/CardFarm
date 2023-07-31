using System;
using System.Collections.Generic;
using System.Linq;
using CameraManagement.CameraAim.Core;
using Cards.Core;
using Cards.Data;
using CardsTable.Core;
using Extensions;
using Providers.Graphics;
using Quests.Data;
using UnityEngine;
using Zenject;

namespace Quests.Logic
{
    public class QuestTargetCardsDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        [Header("Preferences")]
        [SerializeField] private List<CardQuantityPair> _targetCards = new List<CardQuantityPair>();
        [SerializeField] private float _outlineShowTime = 2f;
        [SerializeField] private float _cardJumpDuration = 2f;

        [Header("Zoom Preferences")]
        [SerializeField] private float _minAverageDistance = 10f;
        [SerializeField] private float _maxAverageDistance = 20f;
        [SerializeField] private float _minZoomDistance = 10f;
        [SerializeField] private float _maxZoomDistance = 20f;
        [SerializeField] private AnimationCurve _cameraDistanceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private CardSelector _cardSelector;
        private Camera _camera;
        private CameraAimer _cameraAimer;

        [Inject]
        private void Constructor(CardSelector cardSelector,
            CameraProvider cameraProvider,
            CameraAimer cameraAimer)
        {
            _cardSelector = cardSelector;
            _camera = cameraProvider.Value;
            _cameraAimer = cameraAimer;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _questData = GetComponentInParent<QuestData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _questData.Callbacks.OnCLicked += OnQuestClicked;
        }

        private void StopObserving()
        {
            _questData.Callbacks.OnCLicked -= OnQuestClicked;
        }

        private void OnQuestClicked()
        {
            List<CardData> foundCards = FindTargetCards();

            if (foundCards.Count == 0) return;

            Vector3 center = GetCardsCenter(foundCards);
            float distance = GetTargetCameraDistance(center, foundCards);

            _cameraAimer.Aim(center, distance);

            ShowOutlines(foundCards);
            PlayAnimations(foundCards);
        }

        private List<CardData> FindTargetCards()
        {
            List<CardData> targetCards = new List<CardData>();
            Vector3 cameraPosition = _camera.transform.position;

            foreach (var targetCardPair in _targetCards)
            {
                int count = targetCardPair.Quantity;
                List<CardData> foundCards = new List<CardData>();

                if (_cardSelector.SelectedCardsMap.TryGetValue(targetCardPair.Card, out var cards))
                {
                    List<(CardData, float)> cardDistancePairs = new List<(CardData, float)>(cards.Count);

                    foreach (var card in cards)
                    {
                        float distance = Vector3.Distance(cameraPosition, card.transform.position);
                        cardDistancePairs.Add((card, distance));
                    }

                    foundCards = cardDistancePairs.OrderBy(x => x.Item2).Take(count).Select(x => x.Item1).ToList();
                }

                targetCards.AddRange(foundCards);
            }

            return targetCards;
        }

        private Vector3 GetCardsCenter(List<CardData> cards)
        {
            Vector3 sum = Vector3.zero;

            foreach (var card in cards)
            {
                sum += card.transform.position;
            }

            return sum / cards.Count;
        }

        private float GetTargetCameraDistance(Vector3 center, List<CardData> cards)
        {
            float distanceSum = 0f;
            float targetCameraDistance = 0f;

            for (int i = 1; i < cards.Count; i++)
            {
                CardData previousCard = cards[i - 1];
                CardData currentCard = cards[i];

                float distance = Vector3.Distance(previousCard.transform.position, currentCard.transform.position);
                distanceSum += distance;
            }

            targetCameraDistance = _cameraDistanceCurve.Evaluate(_minAverageDistance, _maxAverageDistance, distanceSum / cards.Count,
                _minZoomDistance, _maxZoomDistance);

            return targetCameraDistance;
        }

        private void ShowOutlines(List<CardData> cards)
        {
            foreach (var card in cards)
            {
                card.QuestOutline.Show(_outlineShowTime);
            }
        }

        private void PlayAnimations(List<CardData> cards)
        {
            foreach (var card in cards)
            {
                if (card.IsTakingPartInRecipe.Value == false && card.IsSingleCard.Value)
                {
                    card.Animations.ContinuousJumpingAnimation.PlayContinuous(_cardJumpDuration);
                }
            }
        }

        [Serializable]
        public class CardQuantityPair
        {
            public Card Card;
            public int Quantity = 1;
        }
    }
}
