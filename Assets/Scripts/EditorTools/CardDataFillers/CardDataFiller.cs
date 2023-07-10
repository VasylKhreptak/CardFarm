using Cards.Data;
using Cards.Gestures.PositionShake;
using Cards.Graphics.Animations;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using NaughtyAttributes;
using Tags.Cards;
using UnityEngine;
using Zenject;

namespace EditorTools.CardDataFillers
{
    public class CardDataFiller : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            FillData();
        }

        #endregion

        [Button]
        protected virtual void FillData()
        {
            _cardData = GetComponent<CardData>();

            if (_cardData == null) return;

            _cardData.MouseTrigger = GetComponentInChildren<ObservableMouseTrigger>();

            BottomCardFollowPoint bottomCardFollowPoint = GetComponentInChildren<BottomCardFollowPoint>();
            _cardData.BottomCardFollowPoint = bottomCardFollowPoint != null ? bottomCardFollowPoint.transform : null;

            _cardData.Collider = GetComponentInChildren<Collider>();

            _cardData.PositionShakeObserver = GetComponentInChildren<PositionShakeObserver>();

            _cardData.Animations.MoveAnimation = GetComponentInChildren<CardMoveAnimation>();
            _cardData.Animations.JumpAnimation = GetComponentInChildren<CardJumpAnimation>();
            _cardData.Animations.FlipAnimation = GetComponentInChildren<CardFlipAnimation>();
            _cardData.Animations.ShakeAnimation = GetComponentInChildren<CardShakeAnimation>();
        }
    }
}
