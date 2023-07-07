using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Food.Data;
using Cards.Workers.Data;
using Data.Days;
using Extensions;
using NaughtyAttributes;
using Table.Core;
using Table.ManualCardSelectors;
using UnityEngine;
using Zenject;

namespace Runtime.Workers
{
    public class WorkersFeeder : MonoBehaviour
    {
        private Coroutine _feedWorkersCoroutine;

        private DaysData _daysData;
        private WorkersSelector _workersSelector;
        private FoodSelector _foodSelector;
        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(DaysData daysData,
            WorkersSelector workersSelector,
            FoodSelector foodSelector,
            CardsTable cardsTable)
        {
            _daysData = daysData;
            _workersSelector = workersSelector;
            _foodSelector = foodSelector;
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopFeedingWorkers();
        }

        #endregion

        private void StartObserving()
        {
            _daysData.Callbacks.onNewDayCome += StartFeedingWorkers;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= StartFeedingWorkers;
        }

        [Button]
        private void StartFeedingWorkers()
        {
            StopFeedingWorkers();
            _feedWorkersCoroutine = StartCoroutine(FeedWorkersRoutine());
        }

        private void StopFeedingWorkers()
        {
            if (_feedWorkersCoroutine != null)
            {
                StopCoroutine(_feedWorkersCoroutine);
            }
        }

        private IEnumerator FeedWorkersRoutine()
        {
            List<WorkerData> workerCards = _workersSelector.SelectedCards.Select(x => x as WorkerData).ToList();

            DecreaseWorkersSatiety(workerCards);

            if (_foodSelector.SelectedCards.Count == 0) yield break;

            List<FoodCardData> foodCards = _foodSelector.SelectedCards.Select(x => x as FoodCardData).ToList();

            FoodCardData lastMovedFood = null;

            List<FoodCardData> foodToRemove = new List<FoodCardData>();

            foreach (var worker in workerCards)
            {
                while (worker.NeededSatiety.Value > 0)
                {
                    foodToRemove.Clear();

                    foreach (var food in foodCards)
                    {
                        if (food.gameObject.activeSelf == false) continue;

                        float foodMoveDuration = food.Animations.MoveAnimation.Duration;

                        food.Animations.MoveAnimation.Play(worker.transform.position, () =>
                        {
                            int workerNeededSatiety = worker.NeededSatiety.Value;
                            worker.Satiety.Value += food.NutritionalValue.Value;
                            food.NutritionalValue.Value -= workerNeededSatiety;
                        });

                        lastMovedFood = food;

                        yield return new WaitForSeconds(foodMoveDuration);

                        if (lastMovedFood != null && lastMovedFood.gameObject.activeSelf == false)
                        {
                            foodToRemove.Add(lastMovedFood);
                        }
                        
                        if (worker.NeededSatiety.Value <= 0)
                        {
                            break;
                        }
                    }

                    foreach (var cardToRemove in foodToRemove)
                    {
                        foodCards.Remove(cardToRemove);
                    }
                    
                    if (foodCards.Count == 0) yield break;
                }
            }

            TryLinkCardToGroup(lastMovedFood);
        }

        private void DecreaseWorkersSatiety(List<WorkerData> workers)
        {
            foreach (var worker in workers)
            {
                worker.Satiety.Value = worker.MinSatiety.Value;
            }
        }

        private void TryLinkCardToGroup(CardData cardData)
        {
            if (_cardsTable.TryGetLowestUniqRecipeFreeGroupCard(cardData, out var lastGroupCard))
            {
                cardData.LinkTo(lastGroupCard);
            }
        }
    }
}
