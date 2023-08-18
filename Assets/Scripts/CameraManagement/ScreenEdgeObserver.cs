using UniRx;
using UnityEngine;

namespace CameraManagement
{
    public class ScreenEdgeObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField, Range(0f, 1f)] private float _leftSize = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _rightSize = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _upperSize = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _bottomSize = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _sphereRadius = 0.1f;
        [SerializeField] private Vector2 _sphereCenter = Vector2.zero;

        private FloatReactiveProperty _offset = new FloatReactiveProperty(0f);

        public float LeftSize => _leftSize;
        public float RightSize => _rightSize;
        public float UpperSize => _upperSize;
        public float BottomSize => _bottomSize;
        public float SphereRadius => _sphereRadius;

        public IReadOnlyReactiveProperty<float> Offset => _offset;

        #region MonoBehaviour

        private void Update()
        {
            UpdateOffset();
        }

        #endregion

        private void UpdateOffset()
        {
            int touchCount = Input.touchCount;

            if (touchCount == 0)
            {
                _offset.Value = 0f;
                return;
            }

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            
            
        }
    }
}
