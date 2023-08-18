using CBA.Actions.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class CompleteAllAnimation : Action
    {
        [Header("Callback")]
        [SerializeField] private bool _withCallback;

        public override void Do()
        {
            DOTween.CompleteAll(_withCallback);
        }
    }
}
