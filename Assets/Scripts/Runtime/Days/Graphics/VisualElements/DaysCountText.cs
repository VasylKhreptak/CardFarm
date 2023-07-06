using System;
using Data.Days;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Runtime.Days.Graphics.VisualElements
{
    public class DaysCountText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _daysCountSubscription;

        private DaysData _daysData;

        [Inject]
        private void Constructor(DaysData daysData)
        {
            _daysData = daysData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
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
            _daysCountSubscription = _daysData.Days.Subscribe(SetDaysCount);
        }

        private void StopObserving()
        {
            _daysCountSubscription?.Dispose();
        }

        private void SetDaysCount(int daysCount)
        {
            _tmp.text = daysCount.ToString();
        }
    }
}
