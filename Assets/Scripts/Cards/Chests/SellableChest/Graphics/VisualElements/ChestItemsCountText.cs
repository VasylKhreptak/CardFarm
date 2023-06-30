using System;
using Cards.Chests.SellableChest.Data;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Chests.SellableChest.Graphics.VisualElements
{
    public class ChestItemsCountText : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private ChestSellableCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _tmp = GetComponent<TMP_Text>();
            _cardData = GetComponentInParent<ChestSellableCardData>(true);
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
            _subscription = _cardData.StoredCards.ObserveCountChanged().Subscribe(SetText);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void SetText(int count)
        {
            _tmp.text = count.ToString();
        }
    }
}
