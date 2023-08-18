using CBA.Actions.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody2D
{
    public class AddRigidbody2DTorque : Rigidbody2DAction
    {
        [Header("Preferences")]
        [SerializeField] private float _torque;
        [SerializeField] private ForceMode2D _forceMode = ForceMode2D.Force;

        public override void Do()
        {
            _rigidbody2D.AddTorque(_torque, _forceMode);
        }
    }
}
