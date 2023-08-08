using Cards.Data;
using ItemsIDManagement;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class GroupIDUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private IDProvider _idProvider;

        [Inject]
        private void Constructor(IDProvider idProvider)
        {
            _idProvider = idProvider;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void Awake()
        {
            UpdateID();
            StartObserving();
        }

        private void OnDestroy()
        {
            StoStopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.Callbacks.onBecameHeadOfGroup += UpdateID;
        }

        private void StoStopObserving()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= UpdateID;
        }

        private void UpdateID()
        {
            int groupID = _idProvider.Value;

            foreach (var card in _cardData.GroupCards)
            {
                card.GroupID.Value = groupID;
            }
        }
    }
}
