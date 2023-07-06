using UniRx;

namespace Data.Game
{
    public class GameData
    {
        public IntReactiveProperty Days = new IntReactiveProperty(1);

        public GameDataCallbacks Callbacks = new GameDataCallbacks();
    }
}
