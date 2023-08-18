using CBA.Actions.Rigidbody.Core;

namespace CBA.Actions.Rigidbody
{
    public class SleepRigidbody : RigidbodyAction
    {
        public override void Do()
        {
            _rigidbody.Sleep();
        }
    }
}
