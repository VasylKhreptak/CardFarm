using Cards.Data;
using ItemsIDManagement;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class GroupIDUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDProvider _idProvider;

        [Inject]
        private void Constructor(IDProvider idProvider)
        {
            _idProvider = idProvider;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StoStopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.Callbacks.onBecameHeadOfGroup += OnBecameHeadOfGroup;
        }

        private void StoStopObserving()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= OnBecameHeadOfGroup;
        }

        private void OnBecameHeadOfGroup()
        {
            int groupID = _idProvider.Value;

            foreach (var card in _cardData.GroupCards)
            {
                card.GroupID.Value = groupID;
            }
        }
    }
}
