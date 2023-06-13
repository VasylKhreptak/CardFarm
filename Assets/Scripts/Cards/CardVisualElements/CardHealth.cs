using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.CardVisualElements
{
    public class CardHealth : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DamageableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _healthSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            StartObservingHealth();
        }

        private void OnDisable()
        {
            StopObservingHealth();
        }

        #endregion

        private void StartObservingHealth()
        {
            StopObservingHealth();
            _healthSubscription = _cardData.Health.Subscribe(SetHealth);
        }

        private void StopObservingHealth()
        {
            _healthSubscription?.Dispose();
        }

        private void SetHealth(int price)
        {
            _tmp.text = name;
        }
    }
}
