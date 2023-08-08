using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class IsCardNewReseter : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private int _lastEnabledFrame;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObserving();

            _lastEnabledFrame = Time.frameCount;
        }

        private void OnDisable()
        {
            StopObserving();

            if (Time.frameCount != _lastEnabledFrame)
            {
                ResetValue();
            }
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _cardData.IsSelected.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.UpperCard.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.BottomCard.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentChanged()
        {
            if (_cardData.IsSelected.Value
                || _cardData.UpperCard.Value != null
                || _cardData.BottomCard.Value != null)
            {
                ResetValue();
            }
        }

        private void ResetValue()
        {
            _cardData.IsNew.Value = false;
        }
    }
}
