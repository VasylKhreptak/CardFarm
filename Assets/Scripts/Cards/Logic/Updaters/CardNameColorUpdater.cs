using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CardNameColorUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] private CardDataHolder _cardData;

        private IDisposable _colorSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<CardDataHolder>(true);
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
            StopObserving();
            _colorSubscription = _cardData.NameColor.Subscribe(SetColor);
        }

        private void StopObserving()
        {
            _colorSubscription?.Dispose();
        }

        private void SetColor(Color color)
        {
            _tmp.color = color;
        }
    }
}
