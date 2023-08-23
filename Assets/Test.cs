using DG.Tweening;
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

    private void Awake()
    {
        Sequence mainSequence = DOTween.Sequence();

        Sequence loopedSequence = DOTween.Sequence();

        loopedSequence
            .AppendInterval(0.1f)
            .AppendCallback(() => {})
            .SetLoops(-1, LoopType.Restart);

        mainSequence
            .AppendInterval(5f)
            .Append(loopedSequence)
            .Play();
    }
}
