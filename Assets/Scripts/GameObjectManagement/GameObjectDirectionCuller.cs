using Providers.Graphics;
using UnityEngine;
using Zenject;

namespace GameObjectManagement
{
    public class GameObjectDirectionCuller : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _upDirectionTransform;
        [SerializeField] private GameObject _targetObject;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        public void UpdateCullState()
        {
            if (_camera == null) return;

            Vector3 cameraDirection = _camera.transform.forward;
            Vector3 lookDirection = -_upDirectionTransform.up;

            float dotProduct = Vector3.Dot(cameraDirection, lookDirection);

            bool enabled = dotProduct > 0f;

            _targetObject.SetActive(enabled);
        }
    }
}
