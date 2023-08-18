using CBA.Actions.Management.Transform.Core;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Parent
{
    public class SetParent : TransformAction
    {
        [Header("Preferences")]
        [SerializeField] private UnityEngine.Transform _targetParent;

        public override void Do()
        {
            _transform.SetParent(_targetParent);
        }
    }
}
