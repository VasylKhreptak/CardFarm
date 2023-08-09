using Data.Cards.Core;
using UniRx;
using UnityEngine;

namespace UnlockedCardPanel.VisualizableCard.Data
{
    public class VisualizableCardData : MonoBehaviour
    {
        public ReactiveProperty<CardDataHolder> VisualizableCard = new ReactiveProperty<CardDataHolder>();
    }
}
