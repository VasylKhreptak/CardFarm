using Data.Days;
using UnityEngine;
using Zenject;

namespace Runtime.Days
{
    public class DaysIncrementer : MonoBehaviour
    {
        private DaysData _daysData;

        [Inject]
        private void Constructor(DaysData daysData)
        {
            _daysData = daysData;
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
            _daysData.Callbacks.onNewDayCome += IncrementDay;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= IncrementDay;
        }

        private void IncrementDay()
        {
            _daysData.Days.Value++;
        }
    }
}
