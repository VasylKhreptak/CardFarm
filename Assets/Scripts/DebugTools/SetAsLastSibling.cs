using NaughtyAttributes;
using UnityEngine;

namespace DebugTools
{
    public class SetAsLastSibling : MonoBehaviour
    {
        [Button("Set As Last Sibling")]
        private void Set()
        {
            transform.SetAsLastSibling();
        }
    }
}
