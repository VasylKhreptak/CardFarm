using UnityEngine;

namespace Graphics.Lighting
{
    public class LightData : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private Material _shadowMaterial;

        private static readonly int ColorPropertyName = Shader.PropertyToID("_Color");

        private void OnValidate()
        {
            if (_shadowMaterial == null) return;

            _shadowMaterial.SetColor(ColorPropertyName, ShadowsColor);
        }

        public Vector3 Direction => transform.forward;
        public float MaxLightDistance = 100f;
        public Color ShadowsColor;
    }
}
