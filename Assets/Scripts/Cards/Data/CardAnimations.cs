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
        public CardShakeAnimation ShakeAnimation;
        public CardContinuousJumpingAnimation ContinuousJumpingAnimation;
        public NewCardAppearAnimation AppearAnimation;
        public CardWaveJumpAnimation WaveJumpAnimation;
        public CardNoIntroduceAppearAnimation NoIntroduceAppearAnimation;

        public void Stop()
        {
            MoveAnimation.Stop();
            JumpAnimation.Stop();
            FlipAnimation.Stop();
            ShakeAnimation.Stop();
            ContinuousJumpingAnimation.Stop();
            AppearAnimation.Stop();
            WaveJumpAnimation.Stop();
            NoIntroduceAppearAnimation.Stop();
        }
    }
}
