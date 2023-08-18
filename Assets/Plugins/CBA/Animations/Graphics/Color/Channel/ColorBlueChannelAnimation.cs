using CBA.Animations.Graphics.Color.Channel.Core;
using CBA.Extensions;

namespace CBA.Animations.Graphics.Color.Channel
{
    public class ColorBlueChannelAnimation : ColorChannelAnimation
    {
        protected override float _channel
        {
            get => _colorAdapter.color.b;
            set => _colorAdapter.color = _colorAdapter.color.WithBlue(value);
        }
        
        #region Editor

#if UNITY_EDITOR

        protected override void UpdateColorPreview()
        {
            _startColorPreview = _colorAdapter.color.WithBlue(_from);
            _targetColorPreview = _colorAdapter.color.WithBlue(_to);
        }

#endif

        #endregion

    }
}
