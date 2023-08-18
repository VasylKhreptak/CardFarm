using CBA.Actions.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class RewindAllAnimations : Action
    {
        [Header("Rewind Preferences")]
        [SerializeField] private bool _includeDelay = true;

        public override void Do()
        {
            DOTween.RewindAll(_includeDelay);
        }
    }
}
