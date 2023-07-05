using System;
using Cards.Graphics.Animations;

namespace Cards.Data
{
    [Serializable]
    public class CardAnimations
    {
        public CardMoveAnimation MoveAnimation;
        public CardJumpAnimation JumpAnimation;
        public CardFlipAnimation FlipAnimation;

        public void Stop()
        {
            MoveAnimation.Stop();
            JumpAnimation.Stop();
            FlipAnimation.Stop();
        }
    }
}
