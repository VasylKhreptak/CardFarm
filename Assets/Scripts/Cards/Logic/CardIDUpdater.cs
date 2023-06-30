using Cards.Data;
using ItemsIDManagement;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Logic
{
    public class CardIDUpdater : MonoBehaviour, IValidatable
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

        public void OnValidate()
        {
            _data = GetComponentInParent<CardData>(true);
        }

        private void Awake()
        {
            _data.ID = _idProvider.Value;
        }

        #endregion
    }
}
