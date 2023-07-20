using System;
using UnityEngine;

namespace Economy.Core
{
    public abstract class Bank<T> : MonoBehaviour
    {
        protected T value;

        public T Value => value;

        public Action<T> onChanged;
        public Action<T> onAdded;
        public Action<T> onSpent;
        public Action<T> onFailedSpent;

        public abstract void Add(T value);

        public abstract bool TrySpend(T value);
    }
}
