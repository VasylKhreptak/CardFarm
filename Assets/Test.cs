using Graphics.UI.Particles.Logic;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    public Canvas Canvas;

    private ParticlesPileSpawner _particleSpawner;
    private CoinIconPositionProvider _coinIconPositionProvider;

    [Inject]
    private void Constructor(ParticlesPileSpawner particleSpawner, CoinIconPositionProvider coinIconPositionProvider)
    {
        _particleSpawner = particleSpawner;
        _coinIconPositionProvider = coinIconPositionProvider;
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 startPosition = Input.mousePosition;
    //         Vector3 targetPosition = _coinIconPositionProvider.Value;
    //
    //         _particleSpawner.Spawn(Particle.Coin, Random.Range(1, 31), startPosition, targetPosition);
    //     }
    // }
}
