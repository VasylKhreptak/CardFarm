using CBA.Adapters.Color.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Adapters.Color
{
    public class AdaptedSpriteRendererForColor : ColorAdapter
    {
        [Header("References")]
        [Required, SerializeField] private SpriteRenderer _spriteRenderer;
    
        public override UnityEngine.Color color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }
    
        #region MonoBehaviour

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

        #endregion
    }
}
