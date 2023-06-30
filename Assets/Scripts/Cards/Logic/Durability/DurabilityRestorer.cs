using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Durability
{
    public class DurabilityRestorer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnDisable()
        {
            RestoreDurability();
        }

        #endregion

        private void RestoreDurability()
        {
            _cardData.Durability.Value = _cardData.MaxDurability.Value;
        }
    }
}
