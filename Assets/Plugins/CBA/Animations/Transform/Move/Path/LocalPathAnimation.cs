using System.Linq;
using CBA.Animations.Transform.Move.PositionProvider;
using DG.Tweening;
using UnityEngine;
using CBA.Extensions;

namespace CBA.Animations.Transform.Move.Path
{
    public class LocalPathAnimation : PathAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DOLocalPath(Extensions.PositionProvider.ToVector3Array(_positionProviders), _duration, _pathType, _pathMode, _resolution);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOLocalPath(Extensions.PositionProvider.ToVector3Array(GetReversedPath()), _duration, _pathType, _pathMode, _resolution);
        }

        public override void MoveToStartState()
        {
            _transform.localPosition = _positionProviders.First().position;
        }

        public override void MoveToEndState()
        {
            _transform.localPosition = _positionProviders.Last().position;
        }

        #region Editor

#if UNITY_EDITOR

        protected override void DrawPoints()
        {
            Extensions.Gizmos.DrawPoints(_positionProviders.Select(x => x.transform.position).ToArray(), 0.2f);
        }

        protected override void DrawLines()
        {
            Extensions.Gizmos.DrawLines(_positionProviders.Select(x => x.transform.position).ToArray());
        }

        protected override PositionProvider.Core.PositionProvider AttachPositionProvider(GameObject target)
        {
            return target.AddComponent<TransformLocalPositionProvider>();
        }

#endif

        #endregion

    }
}
