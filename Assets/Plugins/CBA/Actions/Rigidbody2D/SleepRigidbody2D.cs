using CBA.Actions.Rigidbody2D.Core;

namespace CBA.Actions.Rigidbody2D
{
    public class SleepRigidbody2D : Rigidbody2DAction
    {
        public override void Do()
        {
            _rigidbody2D.Sleep();
        }
    }
}
