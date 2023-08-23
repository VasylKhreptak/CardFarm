using Data.Cards.Core;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Graphics.Animations;
using Zenject;

namespace UnlockedCardPanel.VisualizableCard.Data
{
    public class VisualizableCardData : MonoBehaviour, IValidatable
    {
        public ReactiveProperty<CardDataHolder> VisualizableCard = new ReactiveProperty<CardDataHolder>();

        public GameObject CardShirt;

        public VisualizableCardFlipAnimation FLipAnimation;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            FLipAnimation = GetComponentInChildren<VisualizableCardFlipAnimation>(true);
        }

        #endregion
    }
}
