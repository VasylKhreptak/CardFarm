using Graphics.UI.Particles.Coins.Logic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    public Canvas Canvas;

    private CoinsCollector _coinsCollector;
    private CoinsSpender _coinsSpender;

    [Inject]
    private void Constructor(CoinsCollector coinsCollector, CoinsSpender coinsSpender)
    {
        _coinsCollector = coinsCollector;
        _coinsSpender = coinsSpender;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 startPosition = Input.mousePosition;

            _coinsCollector.Collect(Random.Range(1, 21), startPosition);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Vector3 startPosition = Input.mousePosition;

            _coinsSpender.Spend(Random.Range(1, 21), startPosition);
        }
    }
}
