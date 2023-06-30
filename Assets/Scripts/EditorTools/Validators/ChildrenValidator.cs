using System.Collections.Generic;
using System.Linq;
using EditorTools.Validators.Core;
using NaughtyAttributes;
using UnityEngine;

namespace EditorTools.Validators
{
    public class ChildrenValidator : MonoBehaviour, IValidatable
    {
        public void OnValidate()
        {
            ValidateChildren();
        }

        [Button("Validate")]
        private void ValidateChildren()
        {
            List<IValidatable> validatables = transform.GetComponentsInChildren<IValidatable>(true).ToList();
            validatables.Remove(this);

            validatables.ForEach(validator => validator.OnValidate());
        }
    }
}
