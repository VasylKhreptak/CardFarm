using Cards.Data;
using ItemsIDManagement;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardIDUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _data;

        private IDProvider _idProvider;

        [Inject]
        private void Constructor(IDProvider idProvider)
        {
            _idProvider = idProvider;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _data.ID = _idProvider.Value;
        }

        #endregion
    }
}
