using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Graphics.Animations.Shake.Position.Core
{
    public class PositionShakeAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _strength = 0.5f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _snapping = false;
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private ShakeRandomnessMode _randomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease _ease = Ease.Linear;

        private Tween _animation;

        #region MonoBehaviour

        private void OnDisable()
        {
            Stop();
        }

        #endregion

        [Button()]
        public void Play()
        {
            Stop();

            Debug.Log("123123123213");

            _animation = _transform
                .DOShakePosition(_duration, _strength, _vibrato, _randomness, _snapping, _fadeOut, _randomnessMode)
                .SetEase(_ease)
                .Play();
        }

        public void Stop()
        {
            _animation?.Kill();
            _transform.localPosition = Vector3.zero;
        }
    }
}
