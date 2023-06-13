using Providers.Core;

namespace ItemsIDManagement
{
    public class IDProvider : Provider<int>
    {
        private int _previousID = -1;

        public override int Value
        {
            get
            {
                _previousID++;
                return _previousID;
            }
        }
    }
}
