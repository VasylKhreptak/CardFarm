using Providers.Graphics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Runtime.Observers
{
    public class CameraCenterObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private LayerMask _floorLayerMask;

        private Vector3ReactiveProperty _center = new Vector3ReactiveProperty();

        public IReadOnlyReactiveProperty<Vector3> Center => _center;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void Update()
        {
            UpdateCameraCenter();
        }

        #endregion

        private void UpdateCameraCenter()
        {
            RaycastFloor(out RaycastHit hit);
            _center.Value = hit.point;
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Transform cameraTransform = _camera.transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, float.MaxValue, _floorLayerMask);
        }
    }
}
