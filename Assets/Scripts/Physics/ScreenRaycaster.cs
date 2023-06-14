using Providers;
using UnityEngine;
using Zenject;

namespace Physics
{
    public class ScreenRaycaster : MonoBehaviour
    {
        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        public bool Raycast(Vector2 screenPosition, LayerMask mask, out RaycastHit hit)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            return UnityEngine.Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        }
    }
}
