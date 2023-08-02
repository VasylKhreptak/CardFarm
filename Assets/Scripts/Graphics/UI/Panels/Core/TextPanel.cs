using TMPro;
using UnityEngine;

namespace Graphics.UI.Panels.Core
{
    public class TextPanel : Panel
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        public string Text { get => _tmp.text; set => _tmp.text = value; }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponentInChildren<TMP_Text>();
        }

        #endregion
    }
}
