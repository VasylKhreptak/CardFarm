using CameraManagement.CameraMove.Core;
using CameraManagement.CameraZoom.Core;
using DG.Tweening;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraManagement.CameraAim.Core
{
    public class CameraAimer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Zoom Preferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _targetCameraDistance = 10f;
        [SerializeField] private AnimationCurve _zoomCurve;
        [SerializeField] private float _minCameraDistance = 5f;
        [SerializeField] private float _maxCameraDistance = 50f;

        [Header("Move Preferences")]
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("General Preferences")]
        [SerializeField] private float _duration = 1.5f;

        private CompositeDisposable _interruptSubscriptions = new CompositeDisposable();

        private Sequence _aimSequence;

        private MapDragObserver _mapDragObserver;
        private ZoomObserver _zoomObserver;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(MapDragObserver mapDragObserver,
            ZoomObserver zoomObserver,
            GameRestartCommand gameRestartCommand)
        {
            _mapDragObserver = mapDragObserver;
            _zoomObserver = zoomObserver;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += StopAiming;
        }

        private void OnEnable()
        {
            StartObservingInterrupt();
        }

        private void OnDisable()
        {
            StopObservingInterrupt();
            StopAiming();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= StopAiming;
        }

        #endregion

        private void StartObservingInterrupt()
        {
            StopObservingInterrupt();

            _mapDragObserver.IsDragging.Where(x => x).Subscribe(_ => StopAiming()).AddTo(_interruptSubscriptions);
            _zoomObserver.IsZooming.Where(x => x).Subscribe(_ => StopAiming()).AddTo(_interruptSubscriptions);
        }

        private void StopObservingInterrupt()
        {
            _interruptSubscriptions.Clear();
        }

        public void Aim(Transform target)
        {
            Aim(target, _targetCameraDistance, _duration);
        }

        public void Aim(Vector3 targetPosition, float distance)
        {
            Aim(targetPosition, distance, _duration);
        }

        public void Aim(Transform target, float distance, float duration)
        {
            if (_zoomObserver.IsZooming.Value || _mapDragObserver.IsDragging.Value) return;

            distance = Mathf.Clamp(distance, _minCameraDistance, _maxCameraDistance);

            StopAiming();

            Tween moveTween = CreateMoveTween(target, duration);

            Tween zoomTween = CreateZoomTween(distance, duration);

            _aimSequence = DOTween.Sequence()
                .Append(moveTween)
                .Join(zoomTween)
                .Play();
        }

        public void Aim(Vector3 targetPosition)
        {
            Aim(targetPosition, _targetCameraDistance, _duration);
        }

        public void Aim(Vector3 targetPosition, float distance, float duration)
        {
            if (_zoomObserver.IsZooming.Value || _mapDragObserver.IsDragging.Value) return;

            distance = Mathf.Clamp(distance, _minCameraDistance, _maxCameraDistance);

            StopAiming();

            Tween moveTween = CreateMoveTween(targetPosition, duration);

            Tween zoomTween = CreateZoomTween(distance, duration);

            _aimSequence = DOTween.Sequence()
                .Append(moveTween)
                .Join(zoomTween)
                .Play();
        }

        private Tween CreateMoveTween(Transform target, float duration)
        {
            Vector3 startCameraPosition = GetCameraPosition();

            float moveProgress = 0;
            Tween moveTween = DOTween
                .To(() => moveProgress, x => moveProgress = x, 1f, duration)
                .SetEase(_moveCurve)
                .OnUpdate(() =>
                {
                    if (target == null) StopAiming();

                    Vector3 currentCameraPosition = GetCameraPosition();
                    Vector3 targetCameraPosition = target.transform.position;
                    targetCameraPosition.y = currentCameraPosition.y;
                    startCameraPosition.y = currentCameraPosition.y;
                    Vector3 cameraPosition = Vector3.Lerp(startCameraPosition, targetCameraPosition, moveProgress);
                    SetCameraPosition(cameraPosition);
                });

            return moveTween;
        }

        private Tween CreateMoveTween(Vector3 targetPosition, float duration)
        {
            Vector3 startCameraPosition = GetCameraPosition();

            float moveProgress = 0;
            Tween moveTween = DOTween
                .To(() => moveProgress, x => moveProgress = x, 1f, duration)
                .SetEase(_moveCurve)
                .OnUpdate(() =>
                {
                    Vector3 currentCameraPosition = GetCameraPosition();
                    Vector3 targetCameraPosition = targetPosition;
                    targetCameraPosition.y = currentCameraPosition.y;
                    startCameraPosition.y = currentCameraPosition.y;
                    Vector3 cameraPosition = Vector3.Lerp(startCameraPosition, targetCameraPosition, moveProgress);
                    SetCameraPosition(cameraPosition);
                });

            return moveTween;
        }

        private Tween CreateZoomTween(float distance, float duration)
        {
            float startCameraDistance = GetCameraDistance();

            float zoomProgress = 0;
            Tween zoomTween = DOTween
                .To(() => zoomProgress, x => zoomProgress = x, 1f, duration)
                .SetEase(_zoomCurve)
                .OnUpdate(() =>
                {
                    float cameraDistance = Mathf.Lerp(startCameraDistance, distance, zoomProgress);
                    SetCameraDistance(cameraDistance);
                })
                .Play();

            return zoomTween;
        }

        public void StopAiming()
        {
            _aimSequence?.Kill();
        }

        private Vector3 GetCameraPosition()
        {
            return _transform.position;
        }

        private void SetCameraPosition(Vector3 position)
        {
            _transform.position = position;
        }

        private float GetCameraDistance()
        {
            if (RaycastFloor(out var hit))
            {
                return hit.distance;
            }

            return 0;
        }

        private void SetCameraDistance(float distance)
        {
            if (RaycastFloor(out var hit))
            {
                Vector3 hitPoint = hit.point;
                Vector3 direction = -_transform.forward;
                Vector3 targetPosition = hitPoint + direction * distance;
                _transform.position = targetPosition;
            }
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, float.MaxValue, _floorLayerMask);
        }
    }
}
