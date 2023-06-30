using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Logic.Durability
{
    public class DurabilityController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _durabilitySubscription;
        private IDisposable _maxDurabilitySubscription;

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObservingDurability();
        }

        private void OnDisable()
        {
            StopObservingDurability();
        }

        #endregion

        private void StartObservingDurability()
        {
            StopObservingDurability();

            _durabilitySubscription = _cardData.Durability.Subscribe(OnDurabilityChanged);
            _maxDurabilitySubscription = _cardData.MaxDurability.Subscribe(OnDurabilityChanged);
        }

        private void StopObservingDurability()
        {
            _durabilitySubscription?.Dispose();
            _maxDurabilitySubscription?.Dispose();
        }

        private void OnDurabilityChanged(int durability)
        {
            if (durability <= 0)
            {
                _cardData.gameObject.SetActive(false);
            }
            else if (durability > _cardData.MaxDurability.Value)
            {
                _cardData.Durability.Value = _cardData.MaxDurability.Value;
            }
        }
    }
}
