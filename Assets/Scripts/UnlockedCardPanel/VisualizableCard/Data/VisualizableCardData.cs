using Data.Cards.Core;
using GameObjectManagement;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.VisualizableCard.Graphics.Animations;
using Zenject;

namespace UnlockedCardPanel.VisualizableCard.Data
{
    public class VisualizableCardData : MonoBehaviour, IValidatable
    {
        public ReactiveProperty<CardDataHolder> VisualizableCard = new ReactiveProperty<CardDataHolder>();

        public GameObjectDirectionCuller ShirtCuller;

        public VisualizableCardShowAnimation ShowAnimation;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            ShowAnimation = GetComponentInChildren<VisualizableCardShowAnimation>(true);
            ShirtCuller = GetComponentInChildren<GameObjectDirectionCuller>(true);
        }

        #endregion
    }
}
