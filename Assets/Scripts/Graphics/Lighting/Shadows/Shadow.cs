using Graphics.Lighting.Shadows.Core;
using UnityEngine;

namespace Graphics.Lighting.Shadows
{
    public class Shadow : BaseShadow
    {
        [Header("Preferences")]
        [SerializeField] private float _planeHeight;

        private Plane _plane;

        private void Awake()
        {
            _plane = new Plane(Vector3.up, new Vector3(0, _planeHeight, 0));
        }

        protected override void UpdateShadow()
        {
            Vector3 lightDirection = _lightData.Direction;

            Ray ray = new Ray(_rootTransform.position, lightDirection * _lightData.MaxLightDistance);

            if (_plane.Raycast(ray, out var enter))
            {
                _image.enabled = true;

                Vector3 point = ray.GetPoint(enter);
                _transform.position = point;
                _transform.rotation = _rootTransform.rotation;
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
