using Cards.Data;
using Cards.Graphics.Animations;
using Extensions.UniRx.UnityEngineBridge.Triggers;
using NaughtyAttributes;
using Tags.Cards;
using UnityEngine;

namespace Cards.CardDataFillers
{
    public class CardDataFiller : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
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

            _cardData.Animations.MoveAnimation = GetComponentInChildren<CardMoveAnimation>();
            _cardData.Animations.JumpAnimation = GetComponentInChildren<CardJumpAnimation>();
            _cardData.Animations.FlipAnimation = GetComponentInChildren<CardFlipAnimation>();
        }
    }
}
