using UnityEngine;

namespace Data.Holders
{
    public class DataHolder<T> : MonoBehaviour where T : new()
    {
        public T Data = new T();
    }
}
