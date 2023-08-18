using UnityEngine;

namespace CBA.Adapters.Color.Core
{
    public abstract class ColorAdapter : MonoBehaviour
    {
        public abstract UnityEngine.Color color { get; set; }
    }
}
