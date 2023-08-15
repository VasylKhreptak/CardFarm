using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Lighting.Shadows.Core
{
    public abstract class BaseShadow : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] protected Transform _rootTransform;
        [SerializeField] protected Transform _transform;
        [SerializeField] protected Image _image;
        
        protected LightData _lightData;

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
        
        private void Update()
        {
            UpdateShadow();
        }
        
        #endregion
        
        protected abstract void UpdateShadow();
    }
}
