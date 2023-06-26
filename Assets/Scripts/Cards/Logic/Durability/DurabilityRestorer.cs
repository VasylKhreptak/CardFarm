using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Durability
{
    public class DurabilityRestorer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
