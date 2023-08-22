using UniRx;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
using Zenject;

namespace CameraManagement.CameraZoom.Core
{
    public class ZoomObserver : MonoBehaviour
    {
        private FloatReactiveProperty _zoom = new FloatReactiveProperty();
        private BoolReactiveProperty _isZooming = new BoolReactiveProperty();

        private float _lastZoomDistance;

        public IReadOnlyReactiveProperty<float> Zoom => _zoom;
        public IReadOnlyReactiveProperty<bool> IsZooming => _isZooming;

        private NewCardPanel _newCardPanel;

        [Inject]
        private void Constructor(NewCardPanel newCardPanel)
        {
            _newCardPanel = newCardPanel;
        }

        #region MonoBehaviour

        private void Update()
        {
            if (Input.touchCount != 2 || _newCardPanel.IsActive.Value)
            {
                ResetValues();
            }
            else
            {
                OnTwoFingersPressed();
            }

            // #endif
        }

        private void OnDisable()
        {
            ResetValues();
        }

        #endregion

        private void OnTwoFingersPressed()
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
            {
                _lastZoomDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
            }

            float zoomDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
            float delta = zoomDistance - _lastZoomDistance;
            _zoom.Value = delta;
            _lastZoomDistance = zoomDistance;
            _isZooming.Value = true;
        }

        private void ResetValues()
        {
            _lastZoomDistance = 0;
            _zoom.Value = 0;
            _isZooming.Value = false;
        }
    }
}
