using UnityEngine;

namespace TransformManagement
{
    public class LastSiblingSetter : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.SetAsLastSibling();

        }
    }
}
