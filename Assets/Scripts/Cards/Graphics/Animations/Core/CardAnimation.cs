using UniRx;
using UnityEngine;

namespace Cards.Graphics.Animations.Core
{
    public abstract class CardAnimation : MonoBehaviour
    {
        protected BoolReactiveProperty _isPlaying = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying;
    }
}
