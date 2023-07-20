using Graphics.UI.Particles.Coins.Logic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    public Canvas Canvas;

    private CoinsCollector _coinsCollector;

    [Inject]
    private void Constructor(CoinsCollector coinsCollector)
    {
        _coinsCollector = coinsCollector;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 startPosition = Input.mousePosition;

            _coinsCollector.Collect(Random.Range(1, 21), startPosition);
        }
    }
}
