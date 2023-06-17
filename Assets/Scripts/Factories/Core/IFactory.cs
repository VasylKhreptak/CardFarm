namespace Factories.Core
{
    public interface IFactory<Tout>
    {
        public Tout Create();
    }
}
