using Cards.Core;
using ScriptableObjects.Scripts.DataPairs.Core;
using UnityEngine;

namespace ScriptableObjects.Scripts.DataPairs
{
    [CreateAssetMenu(fileName = "CardSpritePairs", menuName = "ScriptableObjects/CardSpritePairs")]
    public class CardSpritePairs : KeyValuePairs<Card, Sprite>
    {

    }
}
