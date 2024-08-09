namespace Innovian.Aspects.Logging.Testing.Aspects
{
    internal class ReturnsNonPrimitiveType
    {
        public Doodad DoSomething()
        {
            return new Doodad();
        }
    }

    internal record Doodad();
}
