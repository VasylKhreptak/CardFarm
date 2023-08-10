using NaughtyAttributes;
using UnityEngine;

namespace DebugTools
{
    public class SetAsFirstSibling : MonoBehaviour
    {
        [Button("Set As First Sibling")]
        private void Set()
        {
            transform.SetAsFirstSibling();
        }
    }
}
