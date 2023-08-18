using CBA.Adapters.Float.Core;
using UnityEngine;

namespace CBA.Adapters.Float
{
    public class AdaptedShaderMaterialForProperty : FloatAdapter
    {
        [Header("References")]
        [SerializeField] private Renderer _renderer;

        [Header("Preferences")]
        [SerializeField] private string _propertyName;

        public override float value
        {
            get => _renderer.sharedMaterial.GetFloat(_propertyName);
            set => _renderer.sharedMaterial.SetFloat(_propertyName, value);
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _renderer ??= GetComponent<Renderer>();
        }

        #endregion
    }
}
