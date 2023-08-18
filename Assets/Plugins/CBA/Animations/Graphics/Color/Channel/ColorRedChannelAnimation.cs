using System;
using CBA.Animations.Graphics.Color.Channel.Core;
using CBA.Extensions;

namespace CBA.Animations.Graphics.Color.Channel
{
    public class ColorRedChannelAnimation : ColorChannelAnimation
    {
        protected override float _channel
        {
            get => _colorAdapter.color.r;
            set => _colorAdapter.color = _colorAdapter.color.WithRed(value);
        }

        #region Editor

#if UNITY_EDITOR
        
        protected override void UpdateColorPreview()
        {
            _startColorPreview = _colorAdapter.color.WithRed(_from);
            _targetColorPreview = _colorAdapter.color.WithRed(_to);
        }
        
#endif

        #endregion
        
    }
}
