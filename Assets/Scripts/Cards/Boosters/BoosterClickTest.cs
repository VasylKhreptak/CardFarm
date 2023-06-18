using Cards.Boosters.Data;
using UnityEngine;

namespace Cards.Boosters
{
    public class BoosterClickTest : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoosterCardData _cardData;

        #region MonoBehaviour

        private void Awake()
        {
            _cardData.OnClick += () => { Debug.Log("Clicked"); };
        }

        #endregion
    }
}
