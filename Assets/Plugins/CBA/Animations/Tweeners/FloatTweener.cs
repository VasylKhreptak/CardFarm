using CBA.Adapters.Float.Core;
using DG.Tweening;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Tweeners
{
    public class FloatTweener : Animation
    {
        [Header("References")]
        [SerializeField] private FloatAdapter _floatAdapter;

        [Header("Tween Preferences")]
        [SerializeField] private float _from;
        [SerializeField] private float _to;

        #region MonoBehaviour

        private void OnValidate()
        {
            _floatAdapter ??= GetComponent<FloatAdapter>();
        }

        #endregion

        public override Tween CreateForwardAnimation() 
        {
            return DOTween.To(() => _floatAdapter.value, x => _floatAdapter.value = x, _to, _duration);
        }
        public override Tween CreateBackwardAnimation()
        {
            return DOTween.To(() => _floatAdapter.value, x => _floatAdapter.value = x, _from, _duration);
        }
        public override void MoveToStartState()
        {
            _floatAdapter.value = _from;
        }
        public override void MoveToEndState()
        {
            _floatAdapter.value = _to;
        }

    }
}
