using Graphics.Lighting.Shadows.Core;
using UnityEngine;

namespace Graphics.Lighting.Shadows
{
    public class PhysicShadow : BaseShadow
    {
        [Header("Preferences")]
        [SerializeField] private LayerMask _layerMask;

        protected override void UpdateShadow()
        {
            Vector3 lightDirection = _lightData.Direction;

            Ray ray = new Ray(_rootTransform.position, lightDirection * _lightData.MaxLightDistance);

            if (UnityEngine.Physics.Raycast(ray, out var hitInfo, _lightData.MaxLightDistance, _layerMask))
            {
                _image.enabled = true;

                _transform.position = hitInfo.point;
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
