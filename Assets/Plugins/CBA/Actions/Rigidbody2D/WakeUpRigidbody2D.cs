using CBA.Actions.Rigidbody2D.Core;

namespace CBA.Actions.Rigidbody2D
{
    public class WakeUpRigidbody2D : Rigidbody2DAction
    {
        public override void Do()
        {
            _rigidbody2D.WakeUp();
        }
    }
}
