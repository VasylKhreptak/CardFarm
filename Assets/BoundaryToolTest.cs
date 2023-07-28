using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Tools.Bounds;
using UnityEngine;

public class BoundaryToolTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _innerRectTransform;
    [SerializeField] private RectTransform _outerRectTransform;
    [SerializeField] private List<RectTransform> _innerRectTransforms;

    private IEnumerator Start()
    {
        while (true)
        {
            Randomize();
    
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        // Debug.Log(_innerRectTransform.IsOverlapping(_innerRectTransforms));
    }

    [Button()]
    private void Randomize()
    {
        _innerRectTransform.position = _outerRectTransform.GetClosestRandomPoint(_innerRectTransforms, _innerRectTransform, _innerRectTransform.position);
    }
}
