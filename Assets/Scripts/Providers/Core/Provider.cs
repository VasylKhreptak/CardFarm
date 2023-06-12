using UnityEngine;

namespace Providers.Core
{
    public abstract class Provider<T> : MonoBehaviour
    {
        public abstract T Value { get; }
    }
}
