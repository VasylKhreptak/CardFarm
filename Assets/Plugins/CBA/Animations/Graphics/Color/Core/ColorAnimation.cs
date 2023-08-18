using CBA.Adapters.Color.Core;
using CBA.Animations.Core;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Graphics.Color.Core
{
    public abstract class ColorAnimation : Animation
    {
        [Header("References")]
        [SerializeField] protected ColorAdapter _colorAdapter;

        #region Monobehavioue

        private void OnValidate()
        {
            _colorAdapter ??= GetComponent<ColorAdapter>();
        }

        #endregion
    }
}
