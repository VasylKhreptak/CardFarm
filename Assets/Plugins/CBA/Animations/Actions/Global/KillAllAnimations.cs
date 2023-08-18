using CBA.Actions.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class KillAllAnimations : Action
    {
        [Header("Complete")]
        [SerializeField] private bool _complete;

        public override void Do()
        {
            DOTween.KillAll(_complete);
        }
    }
}
