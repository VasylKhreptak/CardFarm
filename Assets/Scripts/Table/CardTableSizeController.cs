using System;
using Table.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Table
{
    public class CardTableSizeController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _tableRectTransform;

        [Header("Preferences")]
        [SerializeField] private int _increaseEach;
        [SerializeField] private Vector2 _increaseByPercent;

        private IDisposable _cardsCountChangedSubscription;

        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tableRectTransform ??= GetComponent<RectTransform>();
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
            _cardsCountChangedSubscription = _cardsTable.Cards
                .ObserveCountChanged()
                .DoOnSubscribe(() => OnCardsCountChanged(_cardsTable.Cards.Count))
                .Subscribe(OnCardsCountChanged);
        }

        private void StopObserving()
        {
            _cardsCountChangedSubscription?.Dispose();
        }

        private void OnCardsCountChanged(int count)
        {
            if (count == 0) return;

            if (count % _increaseEach == 0)
            {
                var sizeDelta = _tableRectTransform.sizeDelta;
                sizeDelta.x += sizeDelta.x * _increaseByPercent.x;
                sizeDelta.y += sizeDelta.y * _increaseByPercent.y;
                _tableRectTransform.sizeDelta = sizeDelta;
            }
        }
    }
}
