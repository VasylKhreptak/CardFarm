using System;

namespace Runtime.Commands.Core
{
    public class Command
    {
        public event Action OnBeforeExecute;
        public event Action OnExecute;
        public event Action OnAfterExecute;

        public void Execute()
        {
            OnBeforeExecute?.Invoke();
            OnExecute?.Invoke();
            OnAfterExecute?.Invoke();
        }
    }
}
