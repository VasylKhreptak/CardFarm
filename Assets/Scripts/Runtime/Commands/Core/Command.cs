using System;

namespace Runtime.Commands.Core
{
    public class Command
    {
        public event Action OnExecute;

        public void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}
