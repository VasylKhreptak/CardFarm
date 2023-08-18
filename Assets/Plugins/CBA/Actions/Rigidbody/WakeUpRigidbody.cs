using CBA.Actions.Rigidbody.Core;

namespace CBA.Actions.Rigidbody
{
    public class WakeUpRigidbody : RigidbodyAction
    {
        public override void Do()
        {
            _rigidbody.WakeUp();
        }
    }
}
