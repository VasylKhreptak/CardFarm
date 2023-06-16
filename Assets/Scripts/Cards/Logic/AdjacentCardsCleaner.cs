using Cards.Data;
using Extensions;
using UnityEngine;

namespace Cards.Logic
{
    public class AdjacentCardsCleaner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnDisable()
        {
            _cardData.Separete();
        }

        #endregion
    }
}
