using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Core
{
    public abstract class AnimationCore : MonoBehaviour
    {
        [Header("Animation Preferences")]
        [SerializeField] private bool _useAnimationCurve;
        [HideIf(nameof(_useAnimationCurve)), SerializeField] private Ease _ease = DOTween.defaultEaseType;
        [ShowIf(nameof(_useAnimationCurve)), SerializeField] private AnimationCurve _curve;
        [SerializeField] private bool _useAdditionalSettings;
        [SerializeField] private bool _killOnDisable = true;
        [ShowIf(nameof(_useAdditionalSettings)), SerializeField] private int _id = -999;
        [ShowIf(nameof(_useAdditionalSettings)), SerializeField] private float _delay;
        [ShowIf(nameof(_useAdditionalSettings)), SerializeField] private int _loops = 1;
        [ShowIf(nameof(_useAdditionalSettings)), SerializeField] private LoopType _loopType = DOTween.defaultLoopType;
        [ShowIf(nameof(_useAdditionalSettings)), SerializeField] private UpdateType _updateType = DOTween.defaultUpdateType;

        private Tween _animation;
        
        public Tween Animation => _animation;
        
        public event Action onInit;

        #region MonoBehaviour

        protected virtual void OnDisable()
        {
            if (_killOnDisable)
            {
                Stop();
            }
        }
        
        protected virtual void OnDestroy()
        {
            _animation.Kill();
        }

        #endregion

        protected void ApplyAnimationPreferences()
        {
            if (_useAdditionalSettings)
            {
                _animation.SetId(_id);
                _animation.SetDelay(_delay);
                _animation.SetLoops(_loops, _loopType);
                _animation.SetUpdate(_updateType);
                _animation.SetAutoKill(true);
            }

            if (_useAnimationCurve)
            {
                _animation.SetEase(_curve);
            }
            else
            {
                _animation.SetEase(_ease);
            }
        }

        public void InitForward()
        {
            Stop();
            _animation = CreateForwardAnimation();

            onInit?.Invoke();
        }

        public void InitBackward()
        {
            Stop();
            _animation = CreateBackwardAnimation();

            onInit?.Invoke();
        }

        public abstract Tween CreateForwardAnimation();

        public abstract Tween CreateBackwardAnimation();

        private bool CanPlay()
        {
            return _animation == null || _animation.active == false;
        }

        #region AnimationControl

        public abstract void MoveToStartState();

        public abstract void MoveToEndState();

        public void PlayForward()
        {
            if (CanPlay())
            {
                PlayForwardImmediate();
            }
        }

        public void PlayBackward()
        {
            if (CanPlay())
            {
                PlayBackwardImmediate();
            }
        }

        public virtual void PlayForwardImmediate()
        {
            Stop();
            InitForward();
            ApplyAnimationPreferences();
            _animation.Play();
        }

        public virtual void PlayBackwardImmediate()
        {
            Stop();
            InitBackward();
            ApplyAnimationPreferences();
            _animation.Play();
        }

        public void PlayFromStart()
        {
            if (CanPlay())
            {
                PlayFromStartImmediate();
            }
        }

        public void PlayFromEnd()
        {
            if (CanPlay())
            {
                PlayFromEndImmediate();
            }
        }

        public void PlayFromStartImmediate()
        {
            MoveToStartState();
            PlayForwardImmediate();
        }

        public void PlayFromEndImmediate()
        {
            MoveToEndState();
            PlayBackwardImmediate();
        }

        public void Stop()
        {
            _animation?.Kill();
            _animation = null;
        }
        
        public void PlayCurrentAnimation()
        {
            _animation?.Play();
        }
        
        #endregion
    }
}
