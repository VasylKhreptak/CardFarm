using Cards.Data;
using Cards.Graphics.Animations.Core;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Animations
{
    public class CardWaveJumpAnimation : CardAnimation, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _interval;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private int _numberOfJumps = 1;
        [SerializeField] private AnimationCurve _jumpCurve;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        #endregion

        public void Play()
        {
            
        }

        public void JumpSingle()
        {
            
        }
        
        public override void Stop()
        {
            
        }
    }
}
