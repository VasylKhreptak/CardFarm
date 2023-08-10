using Graphics.UI.Particles.Coins.Logic;
using TMPro;
using UniRx;
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

    private void Awake()
    {
    }
}
