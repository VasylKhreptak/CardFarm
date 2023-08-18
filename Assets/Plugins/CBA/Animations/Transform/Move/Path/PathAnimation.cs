using System.Collections.Generic;
using System.Linq;
using CBA.Animations.Transform.Move.Path.Core;
using CBA.Animations.Transform.Move.PositionProvider;
using CBA.Extensions;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Color = UnityEngine.Color;
using Gizmos = UnityEngine.Gizmos;
using Vector3 = UnityEngine.Vector3;

namespace CBA.Animations.Transform.Move.Path
{
    public class PathAnimation : Core.PathAnimation
    {
        protected List<PositionProvider.Core.PositionProvider> GetReversedPath()
        {
            List<PositionProvider.Core.PositionProvider> position = new List<PositionProvider.Core.PositionProvider>(_positionProviders);
            position.Reverse();

            return position;
        }

        public override Tween CreateForwardAnimation()
        {
            return _transform.DOPath(Extensions.PositionProvider.ToVector3Array(_positionProviders), _duration, _pathType, _pathMode, _resolution);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DOPath(Extensions.PositionProvider.ToVector3Array(GetReversedPath()), _duration, _pathType, _pathMode, _resolution);
        }

        public override void MoveToStartState()
        {
            _transform.position = _positionProviders.First().position;
        }

        public override void MoveToEndState()
        {
            _transform.position = _positionProviders.Last().position;
        }

        #region Editor

#if UNITY_EDITOR

        [Header("Editor")]
        [SerializeField] private bool _selectPositionOnAdd;
        [SerializeField] protected float _newPositionDistance = 3f;
        [SerializeField] private Vector3 _pathCreationDirection = Vector3.forward;
        [Required, SerializeField] protected UnityEngine.Transform _pathHolder;

        [Button("Clear Path")]
        private void ClearPath()
        {
            if (HasPathHolder() == false) return;

            _positionProviders.Clear();
            _pathHolder.DestroyImmediateChildren();
            DestroyImmediate(_pathHolder.gameObject);
        }

        [Button("Create Path Holder")]
        private void CreatePathHolder()
        {
            if (HasPathHolder()) return;

            GameObject pathHolder = new GameObject("Path Holder");
            pathHolder.transform.SetParent(_transform.parent);
            pathHolder.transform.ResetLocal();
            _pathHolder = pathHolder.transform;
        }

        [Button("Move To Start")]
        private void MoveToStart()
        {
            if (IsPathValid() == false) return;

            MoveToStartState();
        }

        [Button("Move To End")]
        private void MoveToEnd()
        {
            if (IsPathValid() == false) return;

            MoveToEndState();
        }

        [Button("Add New Point At Start")]
        private void AddNewPointAtStart()
        {
            if (HasPathHolder() == false) return;

            CreatePoint(true);
        }

        [Button("Add New Point At End")]
        private void AddNewPointAtEnd()
        {
            if (HasPathHolder() == false) return;

            CreatePoint(false);
        }

        [Button("Remove First Point")]
        private void RemoveFirstPoint()
        {
            if (HasPathHolder() == false) return;

            RemovePoint(_positionProviders.First());
        }

        [Button("Remove Last Point")]
        private void RemoveLastPoint()
        {
            if (HasPathHolder() == false) return;

            RemovePoint(_positionProviders.Last());
        }

        private void RemovePoint(PositionProvider.Core.PositionProvider positionProvider)
        {
            DestroyImmediate(positionProvider.gameObject);
            _positionProviders.Remove(positionProvider);
        }

        private PositionProvider.Core.PositionProvider CreatePoint(bool createdFromStart)
        {
            GameObject pathPoint = new GameObject("Path Point");
            pathPoint.transform.SetParent(_pathHolder);
            PositionProvider.Core.PositionProvider positionProvider = AttachPositionProvider(pathPoint);

            UpdatePointPosition(pathPoint.transform, createdFromStart);

            if (createdFromStart)
            {
                pathPoint.transform.SetAsFirstSibling();
                _positionProviders.Insert(0, positionProvider);
            }
            else
            {
                _positionProviders.Add(positionProvider);
            }

            if (_selectPositionOnAdd)
            {
                SelectObject(pathPoint);
            }

            return positionProvider;
        }

        protected virtual PositionProvider.Core.PositionProvider AttachPositionProvider(GameObject target)
        {
            return target.AddComponent<TransformPositionProvider>();
        }

        private void UpdatePointPosition(UnityEngine.Transform pathPoint, bool createdFromStart)
        {
            pathPoint.position = GetPointStartPosition(createdFromStart);
        }

        private bool HasPathHolder() => _pathHolder != null;

        private bool IsPathValid() => _positionProviders.Count != 0 && _positionProviders.All(x => x != null);

        private void SelectObject(GameObject target)
        {
            UnityEditor.Selection.activeObject = target;
        }

        private Vector3 GetPointStartPosition(bool createdFromStart)
        {
            Vector3 lastPosition, previousPosition;

            if (_positionProviders.Count == 0)
            {
                return _transform.position;
            }

            if (_positionProviders.Count == 1)
            {
                lastPosition = _positionProviders[0].transform.position;
                previousPosition = _transform.position;

                if (lastPosition == previousPosition)
                {
                    return _transform.position + ((!createdFromStart).ToDirection() * _pathCreationDirection * _newPositionDistance);
                }
            }
            else if (createdFromStart)
            {
                lastPosition = _positionProviders.First().transform.position;
                previousPosition = _positionProviders[1].transform.position;
            }
            else
            {
                lastPosition = _positionProviders.Last().transform.position;
                previousPosition = _positionProviders[_positionProviders.Count - 2].transform.position;
            }

            Vector3 direction = (lastPosition - previousPosition).normalized;
            return lastPosition + (direction * _newPositionDistance);
        }

        private void OnDrawGizmos()
        {
            if (_transform == null || IsPathValid() == false) return;

            DrawPath();
        }

        private void DrawPath()
        {
            Gizmos.color = Color.red;

            DrawPoints();

            DrawLines();
        }

        protected virtual void DrawPoints()
        {
            Extensions.Gizmos.DrawPoints(_positionProviders.Select(x => x.position).ToArray(), 0.2f);
        }

        protected virtual void DrawLines()
        {
            Extensions.Gizmos.DrawLines(_positionProviders.Select(x => x.position).ToArray());
        }

#endif

        #endregion

    }
}
