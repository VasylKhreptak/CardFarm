using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Graphics.Shaders
{
    public class CircularProgress : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private Image _image;

        [Header("Preferences")]
        [SerializeField] private string _propertyName = "_Progress";
        [SerializeField] private string _colorName = "_Color";

        private Material _material;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _image ??= GetComponent<Image>();
        }

        private void Awake()
        {
            Material material = Instantiate(_image.material);
            _image.material = material;
            _material = material;
        }

        #endregion

        public void SetProgress(float value)
        {
            _material.SetFloat(_propertyName, value);
        }

        public float GetProgress()
        {
            return _material.GetFloat(_propertyName);
        }

        public void SetColor(Color color)
        {
            _material.SetColor(_colorName, color);
        }

        public Color GetColor()
        {
            return _material.GetColor(_colorName);
        }
    }
}
