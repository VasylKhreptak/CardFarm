using Graphics.UI.Particles.Coins.Logic;
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
        // ReactiveCollection<int> collection = new ReactiveCollection<int>();
        //
        // collection.ObserveAdd().Subscribe(value => Debug.Log($"Add: {value}")).AddTo(this);
        // collection.ObserveRemove().Subscribe(value => Debug.Log($"Remove: {value}")).AddTo(this);
        // collection.ObserveReset().Subscribe(value => Debug.Log($"Reset")).AddTo(this);
        //
        // collection.Add(1);
        // collection.Add(2);
        // collection.Remove(1);
        // collection.Clear();

    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 startPosition = Input.mousePosition;
    //
    //         _coinsCollector.Collect(Random.Range(1, 21), startPosition);
    //     }
    //
    //     if (Input.GetMouseButtonDown(2))
    //     {
    //         Vector3 startPosition = Input.mousePosition;
    //
    //         _coinsSpender.Spend(Random.Range(1, 21), startPosition);
    //     }
    // }
}
