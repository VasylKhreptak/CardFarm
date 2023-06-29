using UniRx;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        ReactiveCollection<int> qwqwe = new ReactiveCollection<int>();
        
        qwqwe.ObserveCountChanged().Subscribe(x =>
        {
            Debug.Log(x);
        });
    }
}
