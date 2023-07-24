using Cards.Core;
using Cards.Logic.Spawn;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace DebugTools
{
    public class ManualCardSpawner : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private Card _card;

        private CardSpawner _cardSpawner;

        [Inject]
        private void Constructor(CardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        [Button()]
        private void Spawn()
        {
            _cardSpawner.Spawn(_card, Vector3.zero);
        }
    }
}
