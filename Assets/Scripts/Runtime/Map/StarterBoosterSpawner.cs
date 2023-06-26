using Cards.Core;
using Cards.Logic.Spawn;
using UnityEngine;
using Zenject;

namespace Runtime.Map
{
    public class StarterBoosterSpawner : MonoBehaviour
    {
        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        #region MonoBehaviour

        private void Start()
        {
            _cardSpawner.Spawn(Card.NewWorldBooster, transform.position);
        }

        #endregion
    }
}
