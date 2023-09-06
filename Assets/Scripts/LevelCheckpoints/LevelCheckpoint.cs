using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LevelCheckpoints
{
    public class LevelCheckpoint : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        [Header("Preferences")]
        [SerializeField, Range(0f, 1f)] private float _targetProgressLevel = 0.5f;

        private BoolReactiveProperty _reached = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> Reached => _reached;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _slider.onValueChanged.AddListener(OnLevelProgressChanged);
        }

        private void StopObserving()
        {
            _slider.onValueChanged.RemoveListener(OnLevelProgressChanged);
        }

        private void OnLevelProgressChanged(float progress)
        {
            if (progress < _targetProgressLevel)
            {
                _reached.Value = false;
                return;
            }

            if (progress >= _targetProgressLevel && _reached.Value == false)
            {
                _reached.Value = true;
            }
        }
    }
}
