using System;
using CBA.Animations.Core;
using Graphics.Animations;
using Graphics.Shaders;
using UnityEngine;
using Zenject;

namespace Graphics.VisualElements.Gears
{
    public class GearsData : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private AnimationCore _showAnimation;
        [SerializeField] private AnimationCore _hideAnimation;
        [SerializeField] private ScalePunchAnimation _scalePunchAnimation;
        [SerializeField] private CircularProgress _circularProgress;
        [SerializeField] private AnimationCore _gearsShowAnimation;
        [SerializeField] private AnimationCore _gearsHideAnimation;
        [SerializeField] private AnimationCore _markShowAnimation;
        [SerializeField] private AnimationCore _markHideAnimation;

        public AnimationCore ShowAnimation => _showAnimation;
        public AnimationCore HideAnimation => _hideAnimation;
        public ScalePunchAnimation ScalePunchAnimation => _scalePunchAnimation;
        public CircularProgress CircularProgress => _circularProgress;
        public AnimationCore GearsShowAnimation => _gearsShowAnimation;
        public AnimationCore GearsHideAnimation => _gearsHideAnimation;
        public AnimationCore MarkShowAnimation => _markShowAnimation;
        public AnimationCore MarkHideAnimation => _markHideAnimation;
        
        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _scalePunchAnimation ??= GetComponentInChildren<ScalePunchAnimation>(true);
            _circularProgress ??= GetComponentInChildren<CircularProgress>(true);
        }

        #endregion
    }
}
