using Cards.Data;
using Extensions;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class AdjacentCardsCleaner : MonoBehaviour, IValidatable
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
            _cardData.Separate();
        }

        #endregion
    }
}
