using System;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Graphics
{
    [Serializable]
    public class CardGraphicData
    {
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Color _textColor;
        [SerializeField] private Sprite _icon;
        
        public Color BackgroundColor => _backgroundColor;
        public Color TextColor => _textColor;
        public Sprite Icon => _icon;
    }
}
