using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Food.Data;
using Cards.Workers.Data;
using Data.Days;
using Table.ManualCardSelectors;
using UnityEngine;
using Zenject;

namespace Runtime.Workers
{
    public class WorkersFeeder : MonoBehaviour
    {
        private DaysData _daysData;
        private WorkersSelector _workersSelector;
        private FoodSelector _foodSelector;

        [Inject]
        private void Constructor(DaysData daysData, WorkersSelector workersSelector, FoodSelector foodSelector)
        {
            _daysData = daysData;
            _workersSelector = workersSelector;
            _foodSelector = foodSelector;
        }

        #region MonoBehaviour

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
            _daysData.Callbacks.onNewDayCome += FeedWorkers;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= FeedWorkers;
        }

        private void FeedWorkers()
        {
            if (_foodSelector.SelectedCards.Count == 0) return;

            List<FoodCardData> foodCards = _foodSelector.SelectedCards.Select(x => x as FoodCardData).ToList();
            List<WorkerData> workerCards = _workersSelector.SelectedCards.Select(x => x as WorkerData).ToList();


        }
    }
}
