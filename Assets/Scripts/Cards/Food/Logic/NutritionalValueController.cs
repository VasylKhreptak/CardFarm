using System;
using Cards.Food.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Food.Logic
{
    public class NutritionalValueController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private FoodCardData _foodData;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _foodData = GetComponentInParent<FoodCardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            ResetNutritionalValue();
        }

        #endregion

        private void StartObserving()
        {
            _subscription = _foodData.NutritionalValue.Subscribe(OnNutritionalValueChanged);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnNutritionalValueChanged(int value)
        {
            int nutritionalValue = _foodData.NutritionalValue.Value;
            int targetNutritionalValue = nutritionalValue;
            int maxNutritionalValue = _foodData.MaxNutritionalValue.Value;

            if (nutritionalValue > maxNutritionalValue)
            {
                targetNutritionalValue = maxNutritionalValue;
            }

            if (nutritionalValue < 0)
            {
                targetNutritionalValue = 0;
            }

            _foodData.NutritionalValue.Value = targetNutritionalValue;

            if (targetNutritionalValue == 0)
            {
                _foodData.gameObject.SetActive(false);
            }
        }

        private void ResetNutritionalValue()
        {
            _foodData.NutritionalValue.Value = _foodData.MaxNutritionalValue.Value;
        }
    }
}
