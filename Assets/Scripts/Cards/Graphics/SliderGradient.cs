using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Cards.Graphics
{
    public class SliderGradient : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _image;

        [Header("Preferences")]
        [SerializeField] private Gradient _gradient;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _slider ??= GetComponent<Slider>();
        }

        private void OnEnable()
        {
            OnValueChanged(_slider.value);
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void StopObserving()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            _image.color = _gradient.Evaluate(value);
        }
    }
}
