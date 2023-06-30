using Cards.Data;
using EditorTools.Validators.Core;
using Extensions;
using UnityEngine;

namespace Cards.Logic
{
    public class AdjacentCardsCleaner : MonoBehaviour, IValidatable
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
            _cardData.Separate();
        }

        #endregion
    }
}
