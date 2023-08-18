using CBA.Animations.Transform.Move.Path;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody.Move.Path
{
    public class RigidbodyLocalPathAnimation : LocalPathAnimation
    {
        [Header("Rigidbody")]
        [SerializeField] private UnityEngine.Rigidbody _rigidbody;

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();

            _rigidbody ??= GetComponent<UnityEngine.Rigidbody>();
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _rigidbody.DOLocalPath(Extensions.PositionProvider.ToVector3Array(_positionProviders), _duration, _pathType, _pathMode, _resolution);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody.DOLocalPath(Extensions.PositionProvider.ToVector3Array(GetReversedPath()), _duration, _pathType, _pathMode, _resolution);
        }
    }
}
