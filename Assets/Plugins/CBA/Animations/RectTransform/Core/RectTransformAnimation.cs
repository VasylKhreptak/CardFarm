using CBA.Animations.Core;
using NaughtyAttributes;
using UnityEngine;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.RectTransform.Core
{
    public abstract class RectTransformAnimation : Animation
    {
        [Header("References")]
        [Required, SerializeField] protected UnityEngine.RectTransform _rectTransform;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<UnityEngine.RectTransform>();
        }

        #endregion
    }
}
