using System;
using Cards.Chests.Core.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Cards.Chests.Core.Graphics.VisualElements
{
    public class ChestItemsCountText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ChestCardData _cardData;
        [SerializeField] private TMP_Text _tmp;

        private IDisposable _subscription;

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
            _subscription = _cardData.ItemsCount.Subscribe(SetText);
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
