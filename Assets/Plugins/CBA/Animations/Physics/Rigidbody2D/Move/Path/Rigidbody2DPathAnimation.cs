using CBA.Animations.Transform.Move.Path;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody2D.Move.Path
{
    public class Rigidbody2DPathAnimation : PathAnimation
    {
        [Header("Rigidbody")]
        [SerializeField] private UnityEngine.Rigidbody2D _rigidbody2D;

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();

            _rigidbody2D ??= GetComponent<UnityEngine.Rigidbody2D>();
        }

        #endregion

        public override Tween CreateForwardAnimation()
        {
            return _rigidbody2D.DOPath(Extensions.PositionProvider.ToVector2Array(_positionProviders), _duration, _pathType, _pathMode, _resolution);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _rigidbody2D.DOPath(Extensions.PositionProvider.ToVector2Array(GetReversedPath()), _duration, _pathType, _pathMode, _resolution);
        }
    }
}
