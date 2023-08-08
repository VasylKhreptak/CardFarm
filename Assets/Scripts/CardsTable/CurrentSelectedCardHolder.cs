using Cards.Data;
using UniRx;
using UnityEngine;

namespace CardsTable
{
    public class CurrentSelectedCardHolder : MonoBehaviour
    {
        public ReactiveProperty<CardDataHolder> SelectedCard = new ReactiveProperty<CardDataHolder>();
    }
}
