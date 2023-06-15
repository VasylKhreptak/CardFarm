using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CardNameColorUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] private CardData _cardData;

        private IDisposable _colorSubscription;

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
