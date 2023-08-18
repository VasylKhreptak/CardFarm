using CBA.Adapters.Alpha.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Adapters.Alpha
{
    public class AdaptedCanvasGroupForAlpha : AlphaAdapter
    {
        [Header("References")]
        [Required, SerializeField] private CanvasGroup _canvasGroup;
    
        public override float alpha
        {
            get => _canvasGroup.alpha;
            set => _canvasGroup.alpha = value;
        }
    
        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        #endregion
    }
}
