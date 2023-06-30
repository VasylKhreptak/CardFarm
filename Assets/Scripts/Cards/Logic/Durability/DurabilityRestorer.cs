using Cards.Data;
using EditorTools.Validators.Core;
using UnityEngine;

namespace Cards.Logic.Durability
{
    public class DurabilityRestorer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        public void OnValidate()
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
