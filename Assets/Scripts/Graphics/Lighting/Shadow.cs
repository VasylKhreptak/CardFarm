using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Lighting
{
    public class Shadow : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        [Header("Preferences")]
        [SerializeField] private float _planeHeight;

        private Plane _plane;

        private LightData _lightData;

        [Inject]
        private void Constructor(LightData lightData)
        {
            _lightData = lightData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
            _image ??= GetComponent<Image>();
        }

        private void Awake()
        {
            _plane = new Plane(Vector3.up, new Vector3(0, _planeHeight, 0));
        }

        private void Update()
        {
            UpdateShadow();
        }

        #endregion

        private void UpdateShadow()
        {
            Vector3 lightDirection = _lightData.Direction;

            Ray ray = new Ray(_rootRectTransform.position, lightDirection * _lightData.MaxLightDistance);

            if (_plane.Raycast(ray, out var enter))
            {
                _image.enabled = true;

                Vector3 point = ray.GetPoint(enter);
                _rectTransform.position = point;
            }
            else
            {
                _image.enabled = false;
                _rectTransform.localPosition = Vector3.zero;
                _rectTransform.localRotation = Quaternion.identity;
            }
        }
    }
}
