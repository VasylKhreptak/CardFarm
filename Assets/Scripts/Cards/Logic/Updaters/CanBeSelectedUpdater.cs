using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CanBeSelectedUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
            _cardData.IsInteractable.Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
            _cardData.IsPlayingAnyAnimation.Subscribe(_ => OnStateUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnStateUpdated()
        {
            bool canBeSelected =
                _cardData.IsInteractable.Value
                && _cardData.IsPlayingAnyAnimation.Value == false;

            _cardData.CanBeSelected.Value = canBeSelected;
        }
    }
}
