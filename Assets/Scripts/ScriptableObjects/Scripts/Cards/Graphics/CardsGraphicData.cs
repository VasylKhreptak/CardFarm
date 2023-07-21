using Cards.Core;
using ScriptableObjects.Scripts.DataPairs.Core;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Graphics
{
    [CreateAssetMenu(fileName = "CardsGraphicData", menuName = "ScriptableObjects/CardsGraphicData")]
    public class CardsGraphicData : KeyValuePairs<Card, CardGraphicData>
    {

    }
}
