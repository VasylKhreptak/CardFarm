using CBA.Adapters.Color.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace CBA.Adapters.Color
{
    public class AdaptedTMPForColor : ColorAdapter
    {
        [Header("References")]
        [Required, SerializeField] private TMP_Text _tmpText;
    
        public override UnityEngine.Color color
        {
            get => _tmpText.color;
            set => _tmpText.color = value;
        }
    
        #region MonoBehaviour

        private void OnValidate()
        {
            _tmpText ??= GetComponent<TMP_Text>();
        }

        #endregion
    }
}
