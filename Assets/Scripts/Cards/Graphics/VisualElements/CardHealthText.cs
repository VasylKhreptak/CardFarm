using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.VisualElements
{
    public class CardHealthText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private DamageableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _healthSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<DamageableCardData>(true);
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

        private void SetHealth(int health)
        {
            _tmp.text = health.ToString();
        }
    }
}
