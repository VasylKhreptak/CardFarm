using System;
using Cards.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.VisualElements
{
    public class CardNameText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _nameSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
            _cardData ??= GetComponentInParent<CardData>();
        }

        private void OnEnable()
        {
            StartObservingName();
        }

        private void OnDisable()
        {
            StopObservingName();
        }

        #endregion

        private void StartObservingName()
        {
            StopObservingName();
            _nameSubscription = _cardData.Name.Subscribe(SetName);
        }

        private void StopObservingName()
        {
            _nameSubscription?.Dispose();
        }

        private void SetName(string name)
        {
            if (name == null) return;

            _tmp.text = name;
        }
    }
}
