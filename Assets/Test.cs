using Graphics.UI.Particles.Core;
using Graphics.UI.Particles.Logic;
using NaughtyAttributes;
using Providers.Graphics.UI;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    public Canvas Canvas;

    private ParticleSpawner _particleSpawner;
    private CoinIconPositionProvider _coinIconPositionProvider;

    [Inject]
    private void Constructor(ParticleSpawner particleSpawner, CoinIconPositionProvider coinIconPositionProvider)
    {
        _particleSpawner = particleSpawner;
        _coinIconPositionProvider = coinIconPositionProvider;
    }

    [Button()]
    private void Spawn()
    {
        _particleSpawner.Spawn(Particle.Coin);
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 startPosition = Input.mousePosition;
    //         Vector3 targetPosition = _coinIconPositionProvider.Value;
    //
    //         ParticleData particle = _particleSpawner.Spawn(Particle.Coin, startPosition);
    //
    //         particle.Animations.MoveSequence.Play(targetPosition);
    //     }
    // }
}
