using CBA.Adapters.Alpha.Core;
using CBA.Extensions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace CBA.Adapters.Alpha
{
    public class AdaptedTMPForAlpha : AlphaAdapter
    {
        [Header("References")]
        [Required, SerializeField] private TMP_Text _tmpText;
    
        public override float alpha
        {
            get => _tmpText.color.a;
            set => _tmpText.color = _tmpText.color.WithAlpha(value);
        }
    
        #region MonoBehaviour

        private void OnValidate()
        {
            _tmpText ??= GetComponent<TMP_Text>();
        }

        #endregion
    }
}
