using Cards.Data;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardJoiner : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

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
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.IsSelected.Subscribe(_ => OnEnvironmentUpdated()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentUpdated()
        {
            bool isSelected = _cardData.IsSelected.Value;
            CardDataHolder joinableCard = _cardData.JoinableCard.Value;

            if (isSelected == false)
            {
                if (joinableCard != null)
                {
                    _cardData.LinkTo(joinableCard);
                }
                else
                {
                    _cardData.UnlinkFromUpper();
                }
            }
        }
    }
}
