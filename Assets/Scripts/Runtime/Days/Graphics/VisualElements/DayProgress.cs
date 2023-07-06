using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Days.Graphics.VisualElements
{
    public class DayProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        private IDisposable _dayProgressSubscription;

        private DaysRunner _daysRunner;

        [Inject]
        private void Constructor(DaysRunner daysRunner)
        {
            _daysRunner = daysRunner;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _slider ??= GetComponent<Slider>();
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
            _dayProgressSubscription = _daysRunner.Progress.Subscribe(OnProgressChanged);
        }

        private void StopObserving()
        {
            _dayProgressSubscription?.Dispose();
        }

        private void OnProgressChanged(float progress)
        {
            _slider.value = progress;
        }
    }
}
