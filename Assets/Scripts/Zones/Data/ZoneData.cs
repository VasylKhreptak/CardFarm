using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Zones.Data
{
    public class ZoneData : MonoBehaviour
    {
        public StringReactiveProperty Name = new StringReactiveProperty();
        public UIBehaviour BackgroundBehaviour;
    }
}
