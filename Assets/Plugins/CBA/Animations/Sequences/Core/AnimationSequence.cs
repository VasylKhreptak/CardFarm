using System.Collections.Generic;
using System.Linq;
using CBA.Animations.Core;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Sequences.Core
{
    public class AnimationSequence : AnimationCore
    {
        [Header("Sequence Preferences")]
        [SerializeField] private bool _playSimultaneously;

        [Header("Animations")]
        [SerializeField] private List<Animation> _animations;

        private Sequence _sequence;

        public Sequence Sequence => _sequence;

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_animations == null || _animations.Count == 0)
            {
                _animations = transform.GetComponentsInChildren<Animation>().ToList();
            }
        }

        protected override void OnDestroy()
        {
            _sequence.Kill();
        }

        #endregion

        private List<Animation> GetReversedAnimations()
        {
            List<Animation> animations = new List<Animation>(_animations);
            animations.Reverse();
            return animations;
        }

        public override Tween CreateForwardAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            _sequence = sequence;

            foreach (var animationCore in _animations)
            {
                if (_playSimultaneously)
                {
                    sequence.Join(animationCore.CreateForwardAnimation());
                }
                else
                {
                    sequence.Append(animationCore.CreateForwardAnimation());
                }
            }

            return sequence;
        }

        public override Tween CreateBackwardAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            _sequence = sequence;

            foreach (var animationCore in GetReversedAnimations())
            {
                if (_playSimultaneously)
                {
                    sequence.Join(animationCore.CreateBackwardAnimation());
                }
                else
                {
                    sequence.Append(animationCore.CreateBackwardAnimation());
                }
            }

            return sequence;
        }

        public override void MoveToStartState()
        {
            foreach (var animationCore in _animations)
            {
                animationCore.MoveToStartState();
            }
        }

        public override void MoveToEndState()
        {
            foreach (var animationCore in _animations)
            {
                animationCore.MoveToEndState();
            }
        }

        [Button()]
        public override void PlayForwardImmediate()
        {
            Stop();
            InitForward();
            ApplyAnimationPreferences();
            _sequence.Play();
        }

        [Button()]
        public override void PlayBackwardImmediate()
        {
            Stop();
            InitBackward();
            ApplyAnimationPreferences();
            _sequence.Play();
        }

        public override void Stop()
        {
            _sequence?.Kill();
            _sequence = null;
        }
    }
}
