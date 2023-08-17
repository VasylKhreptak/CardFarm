using Tools.Bounds;
using UnityEngine;

public class BoundaryToolTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _firstRect;
    [SerializeField] private RectTransform _secondRect;

    private void Update()
    {
        Debug.Log(_firstRect.IsOverlapping(_secondRect));
    }
}
