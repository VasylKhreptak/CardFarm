namespace SpawnSystem.Factories.Core
{
    public interface IFactory<Tout>
    {
        Tout Create();
    }
}
