using UniRx;
using UnityEngine;

namespace CameraZoom.Core
{
    public class ZoomObserver : MonoBehaviour
    {
        private FloatReactiveProperty _zoom = new FloatReactiveProperty();

        private float _lastZoomDistance;

        public IReadOnlyReactiveProperty<float> Zoom => _zoom;

        #region MonoBehaviour

        private void Update()
        {
            if (Input.touchCount != 2)
            {
                ResetValues();
                return;
            }

            OnTwoFingersPressed();
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
        }

        private void ResetValues()
        {
            _lastZoomDistance = 0;
            _zoom.Value = 0;
        }
    }
}
