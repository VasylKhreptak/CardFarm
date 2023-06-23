using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.VisualElements
{
    public class CardNutritionalValueText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FoodCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _nutritionalValueSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
            _cardData ??= GetComponentInParent<FoodCardData>();
        }

        private void OnEnable()
        {
            StartObservingNutritionalValue();
        }

        private void OnDisable()
        {
            StopObservingNutritionalValue();
        }

        #endregion

        private void StartObservingNutritionalValue()
        {
            StopObservingNutritionalValue();
            _nutritionalValueSubscription = _cardData.Price.Subscribe(SetNutritionalValue);
        }

        private void StopObservingNutritionalValue()
        {
            _nutritionalValueSubscription?.Dispose();
        }

        private void SetNutritionalValue(int nutritionalValue)
        {
            _tmp.text = nutritionalValue.ToString();
        }
    }
}
