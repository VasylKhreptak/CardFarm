using CBA.Adapters.Color.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Adapters.Color
{
    public class AdaptedImageForColor : ColorAdapter
    {
        [Header("References")]
        [Required, SerializeField] private Image _image;
    
        public override UnityEngine.Color color
        {
            get => _image.color;
            set => _image.color = value;
        }
    
        #region MonoBehaviour

        private void OnValidate()
        {
            _image ??= GetComponent<Image>();
        }

        #endregion
    }
}
