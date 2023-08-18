using CBA.Animations.Physics.Rigidbody.Core;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody.Move.Core
{
    public abstract class RigidbodyMoveAnimation : RigidbodyAnimation
    {
        [Header("Snapping")]
        [SerializeField] protected bool _snapping;
    }
}
