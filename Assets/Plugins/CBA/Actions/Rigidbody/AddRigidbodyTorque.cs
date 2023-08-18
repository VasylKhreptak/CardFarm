using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class AddRigidbodyTorque : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _torque;
        [SerializeField] private ForceMode _forceMode = ForceMode.Force;

        public override void Do()
        {
            _rigidbody.AddTorque(_torque, _forceMode);
        }
    }
}
