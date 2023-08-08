using Cards.Data;
using ItemsIDManagement;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardIDUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _data;

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
            _data = GetComponentInParent<CardDataHolder>(true);
        }

        private void Awake()
        {
            _data.ID = _idProvider.Value;
        }

        #endregion
    }
}
