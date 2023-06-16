using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Graphics.CardVisualElements
{
    public class CardOutlineController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _outlineObject;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _outlineObject ??= transform.GetChild(0).gameObject;
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

            _subscription = _cardData.CanBeParentOfSelectedCard.Subscribe(canBeParent =>
            {
                _outlineObject.SetActive(canBeParent);
            });
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }
    }
}
