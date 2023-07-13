using Extensions;
using Providers;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove.Core
{
    public class MapDragObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private LayerMask _cardLayerMask;

        private Vector2ReactiveProperty _delta = new Vector2ReactiveProperty();

        private Vector2 _previousHitPosition;

        public IReadOnlyReactiveProperty<Vector2> Delta => _delta;

        private Camera _camera;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CameraProvider cameraProvider, CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _camera = cameraProvider.Value;
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void Update()
        {
            if (Input.touchCount != 1 || _currentSelectedCardHolder.SelectedCard.Value != null)
            {
                ResetValues();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButton(0))
            {
                OnMouse();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }

        #endregion

        private void OnMouseDown()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (_cardLayerMask.ContainsLayer(hit.collider.gameObject.layer)) return;

                _previousHitPosition = new Vector2(hit.point.x, hit.point.z);
            }
        }

        private void OnMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (_cardLayerMask.ContainsLayer(hit.collider.gameObject.layer)) return;

                Vector2 hitPoint = new Vector2(hit.point.x, hit.point.z);

                _delta.Value = hitPoint - _previousHitPosition;

                UpdatePreviousPosition();
            }
        }

        private void OnMouseUp()
        {
            ResetValues();
        }

        private void ResetValues()
        {
            _delta.Value = Vector2.zero;
            _previousHitPosition = Vector3.zero;
        }


        private void UpdatePreviousPosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                _previousHitPosition = new Vector2(hit.point.x, hit.point.z);
            }
        }
    }
}
