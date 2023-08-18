using UnityEngine;

namespace CBA.Animations.Transform.Move.PositionProvider
{
    public class TransformLocalPositionProvider : TransformPositionProvider
    {
        public override Vector3 position
        {
            get => _transform.localPosition;
            set => _transform.localPosition = value;
        }
    }
}
