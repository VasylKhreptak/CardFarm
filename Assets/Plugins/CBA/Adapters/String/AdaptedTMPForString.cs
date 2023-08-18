using System;
using CBA.Adapters.String.Core;
using TMPro;
using UnityEngine;

namespace CBA.Adapters.String
{
    public class AdaptedTMPForString : StringAdapter
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        #endregion

        public override string text
        {
            get => _tmp.text;
            set => _tmp.text = value;
        }
    }
}
