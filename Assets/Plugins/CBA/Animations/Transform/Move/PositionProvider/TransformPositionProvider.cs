using UnityEngine;

namespace CBA.Animations.Transform.Move.PositionProvider
{
    public class TransformPositionProvider : Core.PositionProvider
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.Transform _transform;

        public override Vector3 position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<UnityEngine.Transform>();
        }

        #endregion
    }
}
