using CBA.Animations.Physics.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody2D.Move.Core
{
    public abstract class Rigidbody2DMoveAnimation : Rigidbody2DAnimation
    {
        [Header("Snapping")]
        [SerializeField] protected bool _snapping;
    }
}
