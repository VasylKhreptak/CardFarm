using CBA.Animations.Graphics.Color.Channel.Core;
using CBA.Extensions;

namespace CBA.Animations.Graphics.Color.Channel
{
    public class ColorGreenChannelAnimation : ColorChannelAnimation
    {
        protected override float _channel
        {
            get => _colorAdapter.color.g;
            set => _colorAdapter.color = _colorAdapter.color.WithGreen(value);
        }

        #region Editor

#if UNITY_EDITOR
        
        protected override void UpdateColorPreview()
        {
            _startColorPreview = _colorAdapter.color.WithGreen(_from);
            _targetColorPreview = _colorAdapter.color.WithGreen(_to);
        }
        
#endif

        #endregion
        
    }
}
