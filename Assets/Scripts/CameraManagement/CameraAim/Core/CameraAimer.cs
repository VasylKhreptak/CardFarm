using DG.Tweening;
using UnityEngine;

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

        [Header("Move Preferences")]
        [SerializeField] private AnimationCurve _moveCurve;

        [Header("General Preferences")]
        [SerializeField] private float _duration = 1.5f;

        private Sequence _aimSequence;

        #region MonoBehaviour

        private void OnDisable()
        {
            StopAiming();
        }

        #endregion

        public void Aim(Transform target)
        {
            Aim(target, _targetCameraDistance, _duration);
        }

        public void Aim(Transform target, float duration)
        {
            Aim(target, _targetCameraDistance, duration);
        }

        public void Aim(Transform target, float distance, float duration)
        {
            StopAiming();

            Tween moveTween = CreateMoveTween(target, duration);

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
