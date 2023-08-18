using CBA.Adapters.Float.Core;
using UnityEngine;

namespace CBA.Adapters.Float
{
    public class AdaptedMaterialForProperty : FloatAdapter
    {
        [Header("References")]
        [SerializeField] private Renderer _renderer;

        [Header("Preferences")]
        [SerializeField] private string _propertyName;

        public override float value
        {
            get => _renderer.material.GetFloat(_propertyName);
            set => _renderer.material.SetFloat(_propertyName, value);
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _renderer ??= GetComponent<Renderer>();
        }

        #endregion

    }
}
