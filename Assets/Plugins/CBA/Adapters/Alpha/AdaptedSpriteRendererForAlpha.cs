using CBA.Adapters.Alpha.Core;
using CBA.Extensions;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Adapters.Alpha
{
    public class AdaptedSpriteRendererForAlpha : AlphaAdapter
    {
        [Header("References")]
        [Required, SerializeField] private SpriteRenderer _spriteRenderer;

        public override float alpha
        {
            get => _spriteRenderer.color.a;
            set => _spriteRenderer.color = _spriteRenderer.color.WithAlpha(value);
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

        #endregion
    }
}
