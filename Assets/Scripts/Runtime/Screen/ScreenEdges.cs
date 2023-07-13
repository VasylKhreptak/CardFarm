using UniRx;
using UnityEngine;

namespace Runtime.Screen
{
    public class ScreenEdges : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField, Range(0f, 1f)] private float _horizontalPercent;
        [SerializeField, Range(0f, 1f)] private float _verticalPercent;

        private BoolReactiveProperty _isPointerDown = new BoolReactiveProperty(false);
        private Vector2ReactiveProperty _directionFromCenter = new Vector2ReactiveProperty(Vector2.zero);

        public IReadOnlyReactiveProperty<bool> IsPointerDown => _isPointerDown;
        public IReadOnlyReactiveProperty<Vector2> DirectionFromCenter => _directionFromCenter;

        #region MonoBehaviour

        private void Update()
        {
            if (Input.touchCount != 1 || Input.GetMouseButton(0) == false)
            {
                ResetValues();
                return;
            }

            Vector2 mousePosition = Input.mousePosition;
            float halfHorizontalPercent = _horizontalPercent * 0.5f;
            float halfVerticalPercent = _verticalPercent * 0.5f;

            if (mousePosition.x < UnityEngine.Screen.width * halfHorizontalPercent ||
                mousePosition.x > UnityEngine.Screen.width * (1f - halfHorizontalPercent) ||
                mousePosition.y < UnityEngine.Screen.height * halfVerticalPercent ||
                mousePosition.y > UnityEngine.Screen.height * (1f - halfVerticalPercent))
            {
                _isPointerDown.Value = true;
                Vector2 vectorFromCenter = mousePosition - new Vector2(UnityEngine.Screen.width * 0.5f, UnityEngine.Screen.height * 0.5f);
                _directionFromCenter.Value = vectorFromCenter.normalized;
            }
            else
            {
                ResetValues();
            }

        }

        private void OnDisable()
        {
            ResetValues();
        }

        #endregion

        private void ResetValues()
        {
            _isPointerDown.Value = false;
            _directionFromCenter.Value = Vector2.zero;
        }
    }
}
