namespace Factories.Core
{
    public interface IParameterizedFactory<Tin, Tout>
    {
        public Tout Create(Tin input);
    }
}
