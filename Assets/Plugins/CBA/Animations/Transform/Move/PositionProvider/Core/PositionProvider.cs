using UnityEngine;

namespace CBA.Animations.Transform.Move.PositionProvider.Core
{
    public abstract class PositionProvider : MonoBehaviour
    {
        public abstract Vector3 position { get; set; }
    }
}
