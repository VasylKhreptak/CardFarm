using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardDataReseter : MonoBehaviour, IValidatable
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
            ResetData();
        }

        #endregion

        private void ResetData()
        {
            _cardData.IsInteractable.Value = true;
            _cardData.IsPushable.Value = true;
        }
    }
}
