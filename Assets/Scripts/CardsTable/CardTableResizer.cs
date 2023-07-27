using System;
using System.Collections.Generic;
using Cards.Core;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace CardsTable
{
    public class CardTableResizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _tableRectTransform;

        [Header("Preferences")]
        [SerializeField] private int _increaseEach;
        [SerializeField] private Vector2 _increaseByPercent;
        [SerializeField] private List<Card> _blackList;

        private IDisposable _cardsCountChangedSubscription;

        private int _maxCardsCount;
        private Vector2 _initialSizeDelta;

        private GameRestartCommand _gameRestartCommand;
        private Core.CardsTable _cardsTable;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand,
            Core.CardsTable cardsTable)
        {
            _gameRestartCommand = gameRestartCommand;
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tableRectTransform ??= GetComponent<RectTransform>();
        }

        private void Awake()
        {
            _initialSizeDelta = _tableRectTransform.sizeDelta;
            _gameRestartCommand.OnExecute += ResetSize;
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= ResetSize;
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
            count = PassThroughBlackList(count);

            if (count == 0)
            {
                _maxCardsCount = 0;
                return;
            }

            if (count > _maxCardsCount)
            {
                _maxCardsCount = count;
            }
            else
            {
                return;
            }

            if (_maxCardsCount % _increaseEach == 0)
            {
                var sizeDelta = _tableRectTransform.sizeDelta;
                sizeDelta.x += sizeDelta.x * _increaseByPercent.x;
                sizeDelta.y += sizeDelta.y * _increaseByPercent.y;
                _tableRectTransform.sizeDelta = sizeDelta;
            }
        }

        private int PassThroughBlackList(int count)
        {
            int blacklistCardsCount = 0;

            foreach (var card in _cardsTable.Cards)
            {
                if (_blackList.Contains(card.Card.Value))
                {
                    blacklistCardsCount++;
                }
            }

            return Mathf.Max(0, count - blacklistCardsCount);
        }

        private void ResetSize()
        {
            _maxCardsCount = 0;
            _tableRectTransform.sizeDelta = _initialSizeDelta;
        }
    }
}
