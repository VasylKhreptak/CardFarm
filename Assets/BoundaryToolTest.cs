using System;
using System.Collections;
using NaughtyAttributes;
using Tools.Bounds;
using UnityEngine;

public class BoundaryToolTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _innerRectTransform;
    [SerializeField] private RectTransform _outerRectTransform;

    // private IEnumerator Start()
    // {
    //     while (true)
    //     {
    //         Randomize();
    //     
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    private void Update()
    {
        _innerRectTransform.position = _outerRectTransform.Clamp(_innerRectTransform);
    }

    [Button()]
    private void Randomize()
    {
        _innerRectTransform.position = _outerRectTransform.GetRandomPoint(_innerRectTransform);
    }
}
