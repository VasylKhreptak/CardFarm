using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Lighting
{
    public class Shadow : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private Transform _rootTransform;
        [SerializeField] private Transform _transform;
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
            _transform ??= GetComponent<Transform>();
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

            Ray ray = new Ray(_rootTransform.position, lightDirection * _lightData.MaxLightDistance);

            if (_plane.Raycast(ray, out var enter))
            {
                _image.enabled = true;

                Vector3 point = ray.GetPoint(enter);
                transform.position = point;
            }
            else
            {
                _image.enabled = false;
                _transform.localPosition = Vector3.zero;
                _transform.localRotation = Quaternion.identity;
            }
        }
    }
}
